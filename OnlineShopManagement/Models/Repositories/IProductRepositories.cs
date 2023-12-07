using OnlineShopManagement.Models.Domain;

namespace OnlineShopManagement.Models.Repositories
{
    public interface IProductRepositories
    {
        Product GetbyId (int id);
        IEnumerable<Product> Allproducts ();
        Product Add (Product product);
        Product Update (Product product);
        Product Delete (int id);

    }
}
