using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly WebBlogDbContext _context;

        public BlogPostRepository(WebBlogDbContext context)
        {
            _context = context;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _context.AddAsync(blogPost);
            await _context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPost.ToListAsync();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
