﻿namespace BunBlog.Core.Domain.Settings
{
    /// <summary>
    /// 博客配置
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 配置项代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 配置项值
        /// </summary>
        public string Value { get; set; }
    }
}
