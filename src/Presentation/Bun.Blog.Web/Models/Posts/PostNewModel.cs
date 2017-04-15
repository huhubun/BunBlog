﻿namespace Bun.Blog.Web.Models.Posts
{
    public class PostNewModel
    {
        public string Title { get; set; }

        public string Excerpt { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public int Status { get; set; }

    }
}
