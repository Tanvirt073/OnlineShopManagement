using System.ComponentModel.DataAnnotations;

namespace OnlineShopManagement.ViewModels.Administrator
{
    public class CreateRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
