﻿using BunBlog.API.Models.Categories;
using BunBlog.API.Models.Tags;
using BunBlog.Core.Enums;
using System;
using System.Collections.Generic;

namespace BunBlog.API.Models.Posts
{
    /// <summary>
    /// 用于博文列表的博文 Model（相较于 BlogPostModel，移除了内容节点，避免内容太长导致获取列表时返回结果过大）
    /// </summary>
    public class BlogPostListItemModel
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
        public List<TagModel> TagList { get; set; }

        /// <summary>
        /// 元数据信息
        /// </summary>
        public List<PostMetadataModel> MetadataList { get; set; }

        /// <summary>
        /// 博文类型
        /// </summary>
        public PostType Type { get; set; }
    }
}
