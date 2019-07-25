﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BunBlog.API.Models.Categories
{
    /// <summary>
    /// Category
    /// </summary>
    public class CategoryModel
    {
        /// <summary>
        /// 链接名称
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name { get; set; }
    }
}
