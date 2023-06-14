namespace WebBlog.Models.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string DateAdded { get; set; }
    }
}
