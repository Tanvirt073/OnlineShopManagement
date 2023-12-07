namespace OnlineShopManagement.ViewModels
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPrice {  get; set; }
        public IFormFile? Photo { get; set; }
    }
}
