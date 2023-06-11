using Microsoft.AspNetCore.Identity;

namespace WebBlog.Models.ViewModels
{
    public class AdminViewModel
    {
        public List<IdentityRole>? ModelRoles { get; set; }

        public List<AppUser>? ModelUsers { get; set; }

        public List<Tuple<AppUser, IList<string>>>? ModelUsersAndRoles { get; set; }
    }
}
