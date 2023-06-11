using Microsoft.EntityFrameworkCore;
using WebBlog.Models.Domain;

namespace WebBlog.Repositories
{
    public interface ITagRepository
    {
        public Task<IEnumerable<Tag>> GetAllAsync();

        public Task<Tag> GetAsync(Guid id);

        public Task<Tag> AddAsync(Tag tag);

        public Task<Tag> UpdateAsync(Tag tag);

        public Task<Tag> DeleteAsync(Guid id);
    }
}
