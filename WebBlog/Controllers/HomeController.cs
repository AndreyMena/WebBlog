using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBlog.Models;
using WebBlog.Models.Domain;
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
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ILogger<HomeController> logger, 
                                IBlogPostRepository blogPostRepository, 
                                ITagRepository tagRepository, 
                                ICommentRepository commentRepository,
                                UserManager<AppUser> _userManager,
                                RoleManager<IdentityRole> _roleManager)
        {
            _postsRepository = blogPostRepository;
            _logger = logger;
            _tagRepository = tagRepository;
            _commentRepository = commentRepository;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<IActionResult> Index(string category, int? indexPage, string? email)
        {
            ViewData["indexPage"] = indexPage ?? 1;
            ViewData["category"] = category ?? null;
            ViewData["email"] = email ?? null;
            var blogPosts = await _postsRepository.GetAllAsync();

            if (category != null)
            {
                blogPosts = await _postsRepository.GetByTag(category);
            }
            if (email != null)
            {
                blogPosts = await _postsRepository.GetByAuthor(email);
            }

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

            var authors = await userManager.GetUsersInRoleAsync("Author");

            var homeViewModel = new HomeViewModel
            {
                BlogPosts = blogPostsDetails,
                Tags = tags,
                Authors = authors
            };

            return View(homeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetComments(Guid Id)
        {

            var blogCommentsModel = await _commentRepository.GetCommentsByBlogIdAsync(Id);

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
        public async Task<IActionResult> BlogsByTag(string tag)
        {
            Guid tagBlog = Guid.Parse(tag);
            var listBlogs = await _postsRepository.GetAllAsync();
            listBlogs.Select(p => p.Tags.Where(t =>t.Name == tag));

            return View();
        }

        [HttpGet]
        public IActionResult BlogsByTag()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostComments(HomeViewModel homeViewModel)
        {
            var domainModel = new Comment
            {
                PostId = homeViewModel.Id,
                Description = homeViewModel.CommentDescription,
                Email = homeViewModel.Email,
                DateAdded = DateTime.Now,
            };

            await _commentRepository.AddAsync(domainModel);

            var Id = homeViewModel.Id;
            var blogCommentsModel = await _commentRepository.GetCommentsByBlogIdAsync(Id);

            var blogCommentView = new List<CommentViewModel>();

            foreach (var comment in blogCommentsModel)
            {
                blogCommentView.Add(new CommentViewModel
                {
                    Id = homeViewModel.Id,
                    Description = comment.Description,
                    DateAdded = comment.DateAdded.ToShortDateString(),
                    Email = comment.Email
                });
            }

            return Json(blogCommentView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}