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
using WebBlog.Repositories;

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
    }
}
