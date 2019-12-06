﻿using BunBlog.Core.Domain.Posts;
using BunBlog.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BunBlog.Services.Posts
{
    public interface IPostService
    {
        Task<List<Post>> GetListAsync(int pageIndex = 1, int pageSize = 10, bool orderByPublishedOnDesc = true);

        Task<List<Post>> GetListByTagAsync(int tagId, bool tracking = false);

        Task<Post> GetByIdAsync(int id, bool tracking = false, PostType postType = PostType.Post);

        Task<Post> GetByLinkNameAsync(string linkName, bool tracking = false, PostType postType = PostType.Post);

        Task<Post> PostAsync(Post post);

        Task<Post> EditAsync(Post post);

        Task<bool> LinkNameExists(string linkName, PostType type);
    }
}
