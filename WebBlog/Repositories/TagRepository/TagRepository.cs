using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models.Domain;

namespace WebBlog.Repositories.TagRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly WebBlogDbContext _context;

        public TagRepository(WebBlogDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tag.AddAsync(tag);
            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag> DeleteAsync(Guid id)
        {
            var tagFind = await _context.Tag.FindAsync(id);

            if (tagFind != null)
            {
                _context.Tag.Remove(tagFind);
                await _context.SaveChangesAsync();

                return tagFind;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tag.ToListAsync();
        }

        public async Task<Tag> GetAsync(Guid id)
        {
            return await _context.Tag.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag> UpdateAsync(Tag tag)
        {
            var existingTag = await _context.Tag.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _context.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
