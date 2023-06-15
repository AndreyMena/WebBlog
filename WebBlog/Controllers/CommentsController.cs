using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models.Domain;
using WebBlog.Models.ViewModels;
using WebBlog.Repositories;

namespace WebBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly WebBlogDbContext _context;
        private readonly IBlogPostRepository _blogPostRepository;
        public CommentsController(WebBlogDbContext context, IBlogPostRepository blogPostRepository)
        {
            _context = context;
            _blogPostRepository = blogPostRepository;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comment.ToListAsync();

            var commentsViewModel = new List<CommentListViewModel>();

            foreach(var comment in comments)
            {
                var postTitle = await _blogPostRepository.GetAsync(comment.PostId);
                var commentViewModel = new CommentListViewModel
                {
                    Id = comment.Id,
                    Description = comment.Description,
                    DateAdded = comment.DateAdded,
                    Email = comment.Email,
                    PostId = comment.PostId,
                    PostTitle = postTitle.PageTitle
                };

                commentsViewModel.Add(commentViewModel);
            }
            return View(commentsViewModel);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'WebBlogDbContext.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(Guid id)
        {
          return _context.Comment.Any(e => e.Id == id);
        }
    }
}
