using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebBlog.Models;

namespace WebBlog.Data
{
    public class WebBlogContext : DbContext
    {
        public WebBlogContext (DbContextOptions<WebBlogContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Post { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Post");
        }
    }
}
