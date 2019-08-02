﻿using AutoMapper;
using BunBlog.API.Models.Posts;
using BunBlog.Core.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BunBlog.API.MappingProfiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreateBlogPostModel, Post>()
                .ForMember(p => p.Category, mo => mo.Ignore());

            CreateMap<Post, BlogPostModel>()
                .ForMember(m => m.Tags, mo => mo.MapFrom(p => p.PostTags));
        }
    }
}
