﻿using BunBlog.Core.Domain.Tags;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BunBlog.Services.Tags
{
    public interface ITagService
    {
        Task<List<Tag>> GetListAsync();

        Task<Tag> GetByLinkNameAsync(string linkName, bool noTracking = true);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag> EditAsync(Tag tag);


    }
}