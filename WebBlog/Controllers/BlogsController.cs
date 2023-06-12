using Microsoft.AspNetCore.Mvc;
using WebBlog.Models.Domain;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICommentRepository _commentRepository;


        public BlogsController(IBlogPostRepository blogPostRepository, ICommentRepository commentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _commentRepository = commentRepository; 
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);

            var blogCommentsModel = await _commentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

            var blogCommentView = new List<Comment>();

            foreach (var comment in blogCommentsModel)
            {
                blogCommentView.Add(new Comment
                {
                    Description = comment.Description,
                    DateAdded = comment.DateAdded,
                    Email = comment.Email
                });
            }

            var blogDetailsViewModel = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                ShortDescription = blogPost.ShortDescription,
                ImageUrl = blogPost.ImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                Comments = blogCommentView
            };

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetComments(BlogDetailsViewModel blogDetailsViewModel)
        {
            var domainModel = new Comment
            {
                PostId = blogDetailsViewModel.Id,
                Description = blogDetailsViewModel.CommentDescription,
                Email = blogDetailsViewModel.Email,
                DateAdded = DateTime.Now,
            };

            await _commentRepository.AddAsync(domainModel);

            var blogCommentsModel = await _commentRepository.GetCommentsByBlogIdAsync(blogDetailsViewModel.Id);

            var blogCommentView = new List<CommentViewModel>();

            foreach (var comment in blogCommentsModel)
            {
                blogCommentView.Add(new CommentViewModel
                {
                    Description = comment.Description,
                    DateAdded = comment.DateAdded.ToShortDateString(),
                    Email = comment.Email
                });
            }

            return Json(blogCommentView);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            var domainModel = new Comment
            {
                PostId = blogDetailsViewModel.Id,
                Description = blogDetailsViewModel.CommentDescription,
                Email = blogDetailsViewModel.Email,
                DateAdded = DateTime.Now,
            };

            await _commentRepository.AddAsync(domainModel);

            return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
        }
    }
}
