namespace WebBlog.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public Guid PostId { get; set; }
    }
}
