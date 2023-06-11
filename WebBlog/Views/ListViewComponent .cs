using Microsoft.AspNetCore.Mvc;
using WebBlog.Models.Domain;
using WebBlog.Repositories;
using X.PagedList;

namespace WebBlog.Views
{
    public class ListViewComponent : ViewComponent
    {
        private readonly ITagRepository _tagRepository;

        public ListViewComponent(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? indexPage)
        {
            var tags = await _tagRepository.GetAllAsync();
            /*
            return ViewComponent("List",
            new
            {
                indexPage = _indexPage
            });
            */
            return View(tags.ToPagedList(indexPage ?? 1, 5 /*Cantidad de elementos*/));
        }
    }
}
