using Microsoft.EntityFrameworkCore;
using OnlineShopManagement.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                    new Product
                    {
                        Id = 1,
                        ProductName = "Tupi",
                        ProductDescription = "Good Product",
                        PhotoPath = "~/wwwroot/images/default.jpg"
                    }
                );
        }
    }
}
