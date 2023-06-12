using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly WebBlogDbContext _context;

        public CommentRepository(WebBlogDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
           return await _context.Comment.Where(x => x.PostId == blogPostId).ToListAsync();
        }
    }
}
