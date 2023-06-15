using System.ComponentModel.DataAnnotations;

namespace WebBlog.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid PostId { get; set; }
    }
}
