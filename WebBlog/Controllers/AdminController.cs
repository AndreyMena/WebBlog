using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebBlog.Models;
using WebBlog.Models.ViewModels;

namespace WebBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;
        public AdminController(UserManager<IdentityUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> AdminView()
        {
            var roles = roleManager.Roles;
            var users = userManager.Users;

            List<Tuple<IdentityUser, IList<string>>> modelUsersAndRoles =  new List<Tuple<IdentityUser, IList<string>>>();
            //Tuple<IdentityUser, List<IdentityRole>> ModelUsersAndRoles;
            foreach (var item in users) {
                var u = await userManager.GetRolesAsync(item);
                modelUsersAndRoles.Add(new Tuple<IdentityUser, IList<string>>(item, u));
            }
            AdminViewModel adminViewModel = new AdminViewModel()
            { ModelRoles = roles.ToList(), ModelUsers = users.ToList(), ModelUsersAndRoles = modelUsersAndRoles };
            return View(adminViewModel);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserViewModel model)
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
