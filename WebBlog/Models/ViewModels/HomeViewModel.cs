using WebBlog.Models.Domain;

namespace WebBlog.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<BlogDetailsViewModel> BlogPosts { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
    }
}
