using WebBlog.Models.Domain;

namespace WebBlog.Models.ViewModels
{
    public class BlogDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string ImageUrl { get; set; }
        public string PageTitle { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public Boolean Visible { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public int TotalComments { get; set; }

        public string CommentDescription { get; set; }

        public string Email { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
