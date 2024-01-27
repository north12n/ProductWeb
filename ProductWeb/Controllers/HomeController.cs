using Microsoft.AspNetCore.Mvc;
using ProductWeb.Models;
using ProductWeb.ViewModels;
using System.Diagnostics;

namespace ProductWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductContext _productContext;
        public HomeController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public IActionResult Index()
        {
            var products = _productContext.Products.ToList();
            foreach (var item in products)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl))
                {
                    item.ImageUrl = SD.ProductPath + "\\" + item.ImageUrl;
                }

            }
            return View(products);
        }
        public IActionResult Details(int productId)
        {
            return View();
        }
        
    }
}
