using Microsoft.EntityFrameworkCore;
using WebBlog.Models.Domain;

namespace WebBlog.Data
{
    public class WebBlogDbContext : DbContext
    {
        public WebBlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
