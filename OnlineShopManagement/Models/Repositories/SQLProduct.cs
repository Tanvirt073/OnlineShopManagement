using OnlineShopManagement.Models.Domain;

namespace OnlineShopManagement.Models.Repositories
{
    public class SQLProduct : IProductRepositories
    {
        private readonly AppDbContext appDbcontext;

        public SQLProduct(AppDbContext appDbcontext)
        {
            this.appDbcontext = appDbcontext;
        }
        public Product Add(Product product)
        {
            appDbcontext.Products.Add(product);
            appDbcontext.SaveChanges();
            return product;
        }

        public IEnumerable<Product> Allproducts()
        {
            return appDbcontext.Products;
        }

        public Product Delete(int id)
        {
            Product product = appDbcontext.Products.Find(id);
            if (product != null)
            {
                appDbcontext.Products.Remove(product);
                appDbcontext.SaveChanges();
            }
            return product;
        }

        public Product GetbyId(int id)
        {
            return appDbcontext.Products.Find(id);
        }

        public Product Update(Product product)
        {
            var Product = appDbcontext.Products.Attach(product);
            Product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            appDbcontext.SaveChanges();
            return product;
        }
    }
}
