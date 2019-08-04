﻿using BunBlog.API.Models.Categories;
using BunBlog.API.Models.Tags;
using System;
using System.Collections.Generic;

namespace BunBlog.API.Models.Posts
{
    public class BlogPostModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Excerpt { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime PublishedOn { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public CategoryModel Category { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<TagModel> Tags { get; set; }

        /// <summary>
        /// 元数据信息
        /// </summary>
        public List<PostMetadataModel> MetadataList { get; set; }

    }
}
