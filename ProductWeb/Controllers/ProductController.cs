
using Microsoft.AspNetCore.Mvc;

namespace ProductWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _productContext;
        public ProductController(ProductContext productContext)
        {
            _productContext = productContext;
        }
        public IActionResult Index()
        {
            var products = _productContext.Products.Include(p => p.Category).ToList();
            return View(products);
        }


    }
}
