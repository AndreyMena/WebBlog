using Microsoft.AspNetCore.Identity;

namespace WebBlog.Models.ViewModels
{
    public class AdminViewModel
    {
        public List<IdentityRole>? ModelRoles { get; set; }

        public List<IdentityUser>? ModelUsers { get; set; }

        public List<Tuple<IdentityUser, IList<string>>>? ModelUsersAndRoles { get; set; }
    }
}
