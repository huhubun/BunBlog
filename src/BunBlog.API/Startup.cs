﻿using AutoMapper;
using BunBlog.API.Const;
using BunBlog.API.Middleware;
using BunBlog.API.Models;
using BunBlog.Core.Configuration;
using BunBlog.Data;
using BunBlog.Services.Authentications;
using BunBlog.Services.Categories;
using BunBlog.Services.Posts;
using BunBlog.Services.Securities;
using BunBlog.Services.Tags;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace BunBlog.API
{
    public class Startup
    {
        private const string CORS_POLICY_NAME = "BUN_BLOG_API_CORS";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddEntityFrameworkNpgsql().AddDbContext<BunBlogContext>(options =>
            {
                options.UseLazyLoadingProxies().UseNpgsql(Configuration.GetConnectionString("BunBlogConnection"), npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("MigrationHistory");
                });

                // 设置当客户端求值时引发异常
                // https://docs.microsoft.com/zh-cn/ef/core/querying/client-eval#optional-behavior-throw-an-exception-for-client-evaluation
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }).BuildServiceProvider();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Bun Blog API",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            var authenticationConfig = Configuration.GetSection("Authentication").Get<AuthenticationConfig>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,

                    ValidIssuer = authenticationConfig.Issuer,
                    ValidAudience = authenticationConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authenticationConfig.Secret)),

                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostMetadataService, PostMetadataService>();
            services.AddScoped<IBunAuthenticationService, BunAuthenticationService>();
            services.AddScoped<ISecurityService, SecurityService>();

            // appsettings.json 中 Authentication 的配置
            services.AddSingleton<AuthenticationConfig>(service =>
            {
                BunAuthenticationService.AuthenticationConfigValidate(authenticationConfig);
                return authenticationConfig;
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY_NAME, builder => builder.WithOrigins("https://bun.dev", "http://localhost:17088").AllowAnyMethod().AllowAnyHeader());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestResponseLogging();

            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature.Error;

                    var factory = context.RequestServices.GetService<ILoggerFactory>();
                    var logger = factory.CreateLogger("ExceptionHandler");
                    logger.LogError(exception, String.Empty);

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var message = JsonConvert.SerializeObject(new ErrorResponse(ErrorResponseCode.SERVER_ERROR, $"服务器端发生错误，请将 {context.TraceIdentifier} 反馈给管理员以协助修复该问题"));
                    await context.Response.WriteAsync(message);
                });
            });

            app.UseCors(CORS_POLICY_NAME);

            // 必须放在 UseMvc() 的前面
            app.UseAuthentication();

            app.UseMvc();

            // https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Bun Blog API v1");
            });
        }
    }
}
