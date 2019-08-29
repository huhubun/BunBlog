﻿using AutoMapper;
using BunBlog.API.Const;
using BunBlog.API.Models;
using BunBlog.API.Models.Posts;
using BunBlog.Core.Domain.Posts;
using BunBlog.Services.Categories;
using BunBlog.Services.Posts;
using BunBlog.Services.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BunBlog.API.Controllers
{
    /// <summary>
    /// 博文
    /// </summary>
    [Route("api/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly IPostMetadataService _postMetadataService;

        public PostsController(
            IMapper mapper,
            IPostService postService,
            ICategoryService categoryService,
            ITagService tagService,
            IPostMetadataService postMetadataService
            )
        {
            _mapper = mapper;
            _postService = postService;
            _categoryService = categoryService;
            _tagService = tagService;
            _postMetadataService = postMetadataService;
        }

        /// <summary>
        /// 获取博文列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageSize">每页数据条数</param>
        /// <returns>博文列表</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<BlogPostListItemModel>), 200)]
        public async Task<IActionResult> GetListAsync(int page = 1, int pageSize = 10)
        {
            var posts = await _postService.GetListAsync(page, pageSize);

            return Ok(_mapper.Map<List<BlogPostListItemModel>>(posts));
        }

        /// <summary>
        /// 获取一条博文内容
        /// </summary>
        /// <param name="id">博文 id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            // 这里 tracking 设为 true 是因为
            // Lazy-loading is not supported for detached entities or entities that are loaded with 'AsNoTracking()'.
            var post = await _postService.GetByIdAsync(id, tracking: true);

            if (post == null)
            {
                return NotFound(new ErrorResponse(ErrorResponseCode.ID_NOT_FOUND, $"没有 id = {id} 的博文"));
            }

            return Ok(_mapper.Map<BlogPostModel>(post));
        }

        /// <summary>
        /// 创建一条博文
        /// </summary>
        /// <param name="createBlogPostModel">创建博文的请求</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody]CreateBlogPostModel createBlogPostModel)
        {
            var post = _mapper.Map<Post>(createBlogPostModel);

            if (!String.IsNullOrEmpty(createBlogPostModel.Category))
            {
                var category = await _categoryService.GetByLinkNameAsync(createBlogPostModel.Category, tracking: true);
                post.Category = category;
                //post.CategoryId = category.Id;
            }

            if (createBlogPostModel.TagList != null && createBlogPostModel.TagList.Any())
            {
                var tags = await _tagService.GetListByLinkNameAsync(tracking: true, createBlogPostModel.TagList.ToArray());
                post.TagList = tags.Select(t => new PostTag
                {
                    Tag = t
                    //TagId = t.Id
                }).ToList();
            }

            // TODO 用上面注释掉的方式为 id 赋值的话，会导致这里的 post 中导航属性没有值？
            await _postService.PostAsync(post);

            return CreatedAtAction(nameof(GetAsync), new { id = post.Id }, _mapper.Map<BlogPostModel>(post));
        }

        /// <summary>
        /// 修改一条博文
        /// </summary>
        /// <param name="id">博文 id</param>
        /// <param name="editBlogPostModel">修改博文的请求</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody]EditBlogPostModel editBlogPostModel)
        {
            var post = await _postService.GetByIdAsync(id, tracking: true);

            if (post == null)
            {
                return NotFound();
            }

            post = _mapper.Map(editBlogPostModel, post);
            await _postService.EditAsync(post);

            return NoContent();
        }

        /// <summary>
        /// 为指定博文增加访问量
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/visits")]
        public async Task<IActionResult> UpdateVisits([FromRoute]int id)
        {
            var metadata = await _postMetadataService.AddVisitsAsync(id);

            return Ok(_mapper.Map<PostMetadataModel>(metadata));
        }
    }
}