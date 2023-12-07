using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace OnlineShopManagement.ViewModels.Administrator
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();

        }
        
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? ExistingPhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
        public List<string> Claims { get; set; }
        public List<string> Roles { get; set; }
    }
}
