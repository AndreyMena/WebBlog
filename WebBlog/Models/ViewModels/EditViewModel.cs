using System.ComponentModel.DataAnnotations;

namespace WebBlog.Models.ViewModels
{
    public class EditViewModel
    {
        public string Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
