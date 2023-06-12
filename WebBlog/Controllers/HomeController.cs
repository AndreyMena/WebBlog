using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBlog.Models;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogPostRepository _postsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, 
                                IBlogPostRepository blogPostRepository, 
                                ITagRepository tagRepository, 
                                ICommentRepository commentRepository)
        {
            _postsRepository = blogPostRepository;
            _logger = logger;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _postsRepository.GetAllAsync();
            var blogPostsDetails = new List<BlogDetailsViewModel>();

            foreach ( var post in blogPosts )
            {
                var comments = await _commentRepository.GetCommentsByBlogIdAsync(post.Id);
                var commentsCount = comments.Count();

                var postDetails = new BlogDetailsViewModel
                {
                    Id = post.Id,
                    Heading = post.Heading,
                    PageTitle = post.PageTitle,
                    Content = post.Content,
                    ShortDescription = post.ShortDescription,
                    ImageUrl = post.ImageUrl,
                    UrlHandle = post.UrlHandle,
                    PublishedDate = post.PublishedDate,
                    Author = post.Author,
                    Visible = post.Visible,
                    Tags = post.Tags,
                    Comments = post.Comments,
                    TotalComments = commentsCount
                };

                blogPostsDetails.Add(postDetails);
            }
            
            var tags = await _tagRepository.GetAllAsync();

            var homeViewModel = new HomeViewModel
            {
                BlogPosts = blogPostsDetails,
                Tags = tags,
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}