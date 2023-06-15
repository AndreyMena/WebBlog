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
            var existingBlog = await _context.BlogPost.FindAsync(id);
            if (existingBlog != null)
            {
                _context.BlogPost.Remove(existingBlog);
                await _context.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var blogPosts = await _context.BlogPost.Include(x => x.Tags).ToListAsync();

            blogPosts = blogPosts.OrderByDescending(x => x.PublishedDate).ToList();

            return blogPosts;
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            return await _context.BlogPost.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<BlogPost>> GetByTag(string tag)
        {
            var blogs = await _context.BlogPost.Include(x => x.Tags).ToListAsync();
            var blogTags = blogs.Where(b => b.Tags.Any(t => t.Name == tag));
            return blogTags;
        }

        public async Task<IEnumerable<BlogPost>> GetByAuthor(string email)
        {
            var blogs = await _context.BlogPost.Include(x => x.Tags).ToListAsync();
            var blogTags = blogs.Where(b => b.EmailAuthor == email);
            return blogTags.ToList();
        }
        
        public async Task<BlogPost> GetByUrlHandleAsync(string urlHandle)
        {
            return await _context.BlogPost.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await _context.BlogPost.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.ImageUrl = blogPost.ImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await _context.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
