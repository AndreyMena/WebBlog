using WebBlog.Models.Domain;

namespace WebBlog.Models.ViewModels
{
    public class HomeViewModel
    {
        public Guid Id { get; set; }

        public string CommentDescription { get; set; }

        public string Email { get; set; }

        public string UrlHandle { get; set; }

        public Guid CommentId { get; set; }

        public IEnumerable<BlogDetailsViewModel> BlogPosts { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
