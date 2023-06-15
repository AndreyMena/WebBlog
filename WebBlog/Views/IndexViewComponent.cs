using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WebBlog.Models.Domain;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;
using X.PagedList;

namespace WebBlog.Views
{
    public class IndexViewComponent : ViewComponent
    {
        private readonly IBlogPostRepository _postsRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;

        public IndexViewComponent(IBlogPostRepository postRepository, ICommentRepository commentRepository, ITagRepository tagRepository)
        {
            _postsRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category, int? indexPage, string email)
        {

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

            foreach (var post in blogPosts)
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
                BlogDetails = blogPostsDetails.ToPagedList(indexPage ?? 1, 5)
            };

            return View(homeViewModel);
        }
    }
}