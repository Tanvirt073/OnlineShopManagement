using Microsoft.AspNetCore.Identity;

namespace OnlineShopManagement.Models
{
    //Extend the Identity User Class
    public class ApplicationUser: IdentityUser
    {
        public string PhotoPath { get; set; }
    }
}
