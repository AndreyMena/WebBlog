namespace WebBlog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public virtual Post Post { get; set; }
    }
}
