using System.ComponentModel.DataAnnotations;

namespace OnlineShopManagement.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string? ProductPrice { get; set; }
        public string? PhotoPath { get; set; }
    }
}
