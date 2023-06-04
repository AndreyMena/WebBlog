using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebBlog.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> _roleManager) 
        {
            roleManager = _roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
