using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProductWeb.Date;
using System.Security.Claims;

namespace ProductWeb.Controllers
{
    [Authorize] //ต้องล็อกอินแล้วถึงเข้าได้
    public class CartController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty] //ให้ผูกออบเจคโดยอัตโนมัติเทียบเท่าการส่งพารามิเตอร์
		public ShoppingCartVM ShoppingCartVM { get; set; } //[HttpPost]



        public CartController(ProductContext productContext, ShoppingCartService shoppingCartService, IWebHostEnvironment webHostEnvironment)
        {
           _productContext = productContext;
           _shoppingCartService = shoppingCartService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes .NameIdentifier);

            ShoppingCartVM = new()
            {
                ListCart = _productContext.ShoppingCarts.Include(p => p.Product).Where(p => p.UserId == userId).ToList(),
                OrderHeader = new()
            };

            //คำนวนราคารวม
            foreach (var  item in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += item.Product.Price * item.Count;  //item.Product.Price * item.Count = ราคาสินค้า * จำนวน
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Plus(int cartId)  //บวก
        {
            var cart = _productContext.ShoppingCarts.Find(cartId);
            _shoppingCartService.IncrementCount(cart, 1);
            _shoppingCartService.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _productContext.ShoppingCarts.Find(cartId);


            if (cart.Count > 1)
            {
                _shoppingCartService.DecrementCount(cart, 1);
                _shoppingCartService.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _productContext.ShoppingCarts.Find(cartId);
            _productContext.Remove(cart);
            _productContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Summary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _productContext.ShoppingCarts.Include(x => x.Product).Where(x => x.UserId == userId).ToList(),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.User = _productContext.Users.Find(userId);
                
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.User.FullName;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.User.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.User.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.User.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.User.PostalCode;
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        //[HttpPost]
        public IActionResult SummaryPost(/*ShoppingCartVM shoppingCartVM,*/)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            ShoppingCartVM.ListCart = _productContext.ShoppingCarts.Include(x => x.Product).Where(u => u.UserId.Equals(userId));


            ShoppingCartVM.OrderHeader.UserId = userId;
            ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;


            //foreach (var cart in ShoppingCartVM.ListCart)
            //{
            //    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
            //}


            //var user = _productContext.Users.Find(userId);


            #region Image Management
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            var file = ShoppingCartVM.file;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(file.FileName);
                var uploads = wwwRootPath + SD.PaymentPath;


                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);


                //บันทึกรุปภาพใหม่
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                ShoppingCartVM.OrderHeader.PaymentImage = fileName + extension;
            }
            #endregion


            _productContext.OrderHeaders.Add(ShoppingCartVM.OrderHeader); // One
            _productContext.SaveChanges();


            foreach (var cart in ShoppingCartVM.ListCart) // Many
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Count = cart.Count
                };
                _productContext.OrderDetails.Add(orderDetail);
                // productContext.SaveChanges();
            }


            _productContext.ShoppingCarts.RemoveRange(ShoppingCartVM.ListCart); // RemoveRange = ลบตะกร้า 
            _productContext.SaveChanges();


            TempData["success"] = "successful transaction";
            return RedirectToAction("Index", "Home");

        }


    }
}
