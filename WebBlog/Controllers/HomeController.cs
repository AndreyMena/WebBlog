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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _postsRepository = blogPostRepository;
            _logger = logger;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _postsRepository.GetAllAsync();
            var tags = await _tagRepository.GetAllAsync();

            var homeViewModel = new HomeViewModel
            {
                BlogPosts = blogPosts,
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