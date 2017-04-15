﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bun.Blog.Web.Models.Posts
{
    public class EditPostModel
    {
        public string Title { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public int Status { get; set; }
    }
}
