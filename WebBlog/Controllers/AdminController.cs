using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Models;
using WebBlog.Models.ViewModels;

namespace WebBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        public AdminController(UserManager<IdentityUser> _userManager) 
        {
            userManager = _userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> createUser(UserViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) 
                {
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
    }
}
