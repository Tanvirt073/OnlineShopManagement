using Microsoft.AspNetCore.Mvc;
using OnlineShopManagement.Utilities;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopManagement.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")]
        [ValidEmailDomain(allowedDomain:"gmail.com", ErrorMessage ="Email domain must be gmail.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and Confirm Password aren't matching")]
        public string ConfirmPassword { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
    }
}
