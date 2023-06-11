using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using WebBlog.Models;
using WebBlog.Models.ViewModels;

namespace WebBlog.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;
        public AdminController(UserManager<AppUser> _userManager,
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

            List<Tuple<AppUser, IList<string>>> modelUsersAndRoles =  new List<Tuple<AppUser, IList<string>>>();
            //Tuple<AppUser, List<IdentityRole>> ModelUsersAndRoles;
            foreach (var item in users) {
                var u = await userManager.GetRolesAsync(item);
                modelUsersAndRoles.Add(new Tuple<AppUser, IList<string>>(item, u));
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
                var user = new AppUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
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

        [HttpPost]
        public async Task<IActionResult> Delete(AppUser user)
        {
            if (ModelState.IsValid)
            {
                
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);

                    
                    return View();
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditViewModel model, string id)
        {
            if (ModelState.IsValid)
            {
                userManager.FindByEmailAsync(id).Result.FirstName = model.FirstName;
                userManager.FindByEmailAsync(id).Result.LastName = model.LastName;
                var result = await userManager.AddToRoleAsync(userManager.FindByEmailAsync(id).Result, model.Role);

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
