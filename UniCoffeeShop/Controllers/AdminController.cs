using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniCoffeeShop.Models;

namespace UniCoffeeShop.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductDBAccessLayer productDb = new ProductDBAccessLayer();

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageUsers()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageProducts()
        {
            var products = productDb.GetAllProducts();
            return View(products);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct()
        {
            var model = new Product { };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    product.Picture = dataStream.ToArray();
                }
            }

            productDb.AddProduct(product);
            return RedirectToAction("ManageProducts");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditProduct(string productId)
        {
            Product product = productDb.GetProduct(productId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, string productId)
        {
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    product.Picture = dataStream.ToArray();
                }
            }

            productDb.EditProduct(productId, product);
            return RedirectToAction("ManageProducts");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(string productId)
        {
            Product product = productDb.GetProduct(productId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteProduct(Product product, string productId)
        {
            productDb.DeleteProduct(productId);
            return RedirectToAction("ManageProducts");
        }
    }
}
