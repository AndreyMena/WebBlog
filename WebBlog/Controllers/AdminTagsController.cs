using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Models.Domain;

namespace WebBlog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([Bind("Id,Name,DisplayName")] Tag tag)
        {
            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tags = await _tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);

            if (tag != null)
            {
                return View(tag);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,DisplayName")] Tag tag)
        {
            var updatedTag = await _tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {

            }
            else
            {

            }

            // Show error
            return RedirectToAction("List");

        }

        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Id,Name,DisplayName")] Tag tag)
        {
            var deletedTag = await _tagRepository.DeleteAsync(tag.Id);

            if (deletedTag != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new { id = tag.Id });
        }

        //// GET: AdminTags/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null || _context.Tag == null)
        //    {
        //        return NotFound();
        //    }

        //    var tag = await _context.Tag
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tag);
        //}

        //// GET: AdminTags/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AdminTags/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,DisplayName")] Tag tag)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        tag.Id = Guid.NewGuid();
        //        _context.Add(tag);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tag);
        //}

        //// GET: AdminTags/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null || _context.Tag == null)
        //    {
        //        return NotFound();
        //    }

        //    var tag = await _context.Tag.FindAsync(id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(tag);
        //}

        //// POST: AdminTags/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,DisplayName")] Tag tag)
        ////{
        ////    if (id != tag.Id)
        ////    {
        ////        return NotFound();
        ////    }

        ////    if (ModelState.IsValid)
        ////    {
        ////        try
        ////        {
        ////            _context.Update(tag);
        ////            await _context.SaveChangesAsync();
        ////        }
        ////        catch (DbUpdateConcurrencyException)
        ////        {
        ////            if (!TagExists(tag.Id))
        ////            {
        ////                return NotFound();
        ////            }
        ////            else
        ////            {
        ////                throw;
        ////            }
        ////        }
        ////        return RedirectToAction(nameof(Index));
        ////    }
        ////    return View(tag);
        ////}

        //// GET: AdminTags/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null || _context.Tag == null)
        //    {
        //        return NotFound();
        //    }

        //    var tag = await _context.Tag
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tag == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tag);
        //}

        //// POST: AdminTags/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    if (_context.Tag == null)
        //    {
        //        return Problem("Entity set 'WebBlogDbContext.Tag'  is null.");
        //    }
        //    var tag = await _context.Tag.FindAsync(id);
        //    if (tag != null)
        //    {
        //        _context.Tag.Remove(tag);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TagExists(Guid id)
        //{
        //  return _context.Tag.Any(e => e.Id == id);
        ////}
    }
}
