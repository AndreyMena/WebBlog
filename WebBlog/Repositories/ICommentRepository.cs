using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public interface ICommentRepository
    {
        public Task<Comment> AddAsync(Comment comment);

        public Task<IEnumerable<Comment>> GetCommentsByBlogIdAsync(Guid blogPostId);
    }
}
