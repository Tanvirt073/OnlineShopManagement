using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using OnlineShopManagement.Models;
using OnlineShopManagement.Models.Domain;
using OnlineShopManagement.Models.Repositories;
using OnlineShopManagement.ViewModels;
using System.Diagnostics;

namespace OnlineShopManagement.Controllers
{
    public class HomeController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepositories productRepositories;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IProductRepositories productRepositories,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            this.productRepositories = productRepositories;
            this.hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var products = productRepositories.Allproducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string filepath = ProcessUploadedFile(model);
                Product product = new Product()
                {
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    ProductPrice = model.ProductPrice,
                    PhotoPath = filepath
                };
                productRepositories.Add(product);
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = productRepositories.GetbyId(id);
            if (product == null)
            {
                return RedirectToAction("Error");
            }
            ProductEditViewModel model = new ProductEditViewModel()
            {
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ExistingPhotoPath = product.PhotoPath,
            };
            return View(model);
            
        }

        [HttpPost]
        public IActionResult Edit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = productRepositories.GetbyId(model.Id);
                product.ProductName = model.ProductName;
                product.ProductDescription = model.ProductDescription;
                product.ProductPrice = model.ProductPrice;
                if (model.Photo != null)
                {
                    if (product.PhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", product.PhotoPath);
                        System.IO.File.Delete(filePath); 

                    }
                    product.PhotoPath = ProcessUploadedFile(model);
                }
                productRepositories.Update(product);
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Product product = productRepositories.GetbyId(id);
            if (product.PhotoPath != null)
            {
                string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", product.PhotoPath);
                System.IO.File.Delete(filePath);
            }
            productRepositories.Delete(id);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = productRepositories.GetbyId(id);
            if(product == null)
            {
                Response.StatusCode = 404;
                return View("ProductNotFound", id);
            }
            return View(product);
        }

        private string ProcessUploadedFile(ProductCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The Path{exceptionDetails.Path} threw an exception {exceptionDetails.Error}");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}