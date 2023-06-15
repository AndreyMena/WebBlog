using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBlog.Models;
using WebBlog.Models.Domain;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _postRepository;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository postRepository,
            SignInManager<AppUser> _signInManager, UserManager<AppUser> _userManager)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _signInManager = signInManager;
            userManager = _userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            { 
                Tags = tags.Select(x => new  SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var userNamer = User.Identity.Name;
            var appUser = await userManager.FindByNameAsync(userNamer);
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                ImageUrl = addBlogPostRequest.ImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                EmailAuthor = appUser.Email,
                Visible = addBlogPostRequest.Visible,
            };

            var selectedTags = new List<Tag>();

            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;

            await _postRepository.AddAsync(blogPost);
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            if (User.IsInRole("Author")) {
                var userNamer = User.Identity.Name;
                var appUser = await userManager.FindByNameAsync(userNamer);
                var blogPost = await _postRepository.GetByAuthor(appUser.Email);
                return View(blogPost);
            }
            if (User.IsInRole("Admin"))
            {
                var blogPost = await _postRepository.GetAllAsync();
                return View(blogPost);
            }
            return View(null);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userNamer = User.Identity.Name;
            var appUser = await userManager.FindByNameAsync(userNamer);
            var blogPost = await _postRepository.GetAsync(id);
            if (blogPost != null)
            {
                if (blogPost.EmailAuthor == appUser.Email) {
                    var tagsDomainModel = await _tagRepository.GetAllAsync();
                    var model = new EditBlogPostRequest
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
                        Tags = tagsDomainModel.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                        SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                    };
                    return View(model);
                }
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                ShortDescription = editBlogPostRequest.ShortDescription,
                ImageUrl = editBlogPostRequest.ImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Author = editBlogPostRequest.Author,
                Visible = editBlogPostRequest.Visible,
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            var updatedPost = await _postRepository.UpdateAsync(blogPostDomainModel);

            if (updatedPost != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await _postRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }
    }
}
