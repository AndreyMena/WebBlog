﻿using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public interface IBlogPostRepository
    {
        public Task<IEnumerable<BlogPost>> GetAllAsync();

        public Task<BlogPost?> GetAsync(Guid id);

        public Task<BlogPost> AddAsync(BlogPost blogPost);

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        public Task<BlogPost?> DeleteAsync(Guid id);

        public Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        public Task<IEnumerable<BlogPost>> GetByTag(string tag);

        public Task<IEnumerable<BlogPost>> GetByAuthor(string email);
        
    }
}
