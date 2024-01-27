using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductWeb.Models;
using ProductWeb.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace ProductWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductContext _productContext;

        public readonly ShoppingCartService _shoppingCartService;

        public HomeController(ProductContext productContext,ShoppingCartService shoppingCartService)
        {
            _productContext = productContext;
            _shoppingCartService = shoppingCartService;
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
            var product = _productContext.Products.Include(p => p.Category)
                .FirstOrDefault(x => x.Id.Equals(productId));

            if(product == null)
            {
                TempData["message"] = "ไม่พบข้อมูล";
                return RedirectToAction(nameof(Index));
            }

            ShoppingCart shoppingCart = new()
            {
                //Product = _productContext.Products.Find(x => x.ProductId == productId),
                Product = product,  
                Count = 1
            };
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize /*(RoleController = "Customer.Admin")*/] //ตรวจสอบสิทธิ์ตาม role   [ = if (User == null) {} ]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity; //Id ของ User
            //var user = claimsIdentity.FindFirst(claimsIdentity.Name); //ค้น User


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            shoppingCart.UserId = userId;

            var cartFormDb = _productContext.ShoppingCarts.FirstOrDefault(x => x.UserId == shoppingCart.UserId && x.ProductId == shoppingCart.ProductId); 

            if(cartFormDb == null)
            {
                //ยังไม่มีใน ตะกร้า
                _shoppingCartService.Add(shoppingCart);
            }
            else
            {
                //มีใน ตะกร้า 
                _shoppingCartService.IncrementCount(cartFormDb, shoppingCart.Count);
            }

            _shoppingCartService.Save();


            return RedirectToAction(nameof(Index));
        }
    }
}
