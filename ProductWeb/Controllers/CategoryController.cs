using Microsoft.AspNetCore.Mvc;
using ProductWeb.Date;
using System.Security.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductContext _productContext;
        public CategoryController(ProductContext productContext)
        {
            _productContext = productContext;
        }
        public IActionResult Index()
        {
            var result = _productContext.Categories.ToList();
            return View(result);
        }

        //==================================== NEW ==========================================
                                        //ส่ง Id
        public IActionResult UpCreate(int? id)  /*Create & Edit รวมกัน*/
        {
            var category = new Category();

            if (id == null || id == 0)
            {
                //Create

            }
            else
            {
                //Update
                category = _productContext.Categories.Find(id);
                if (category == null)
                {
                    TempData["message"] = "ไม่พบข้อมูล";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);

        }

        [HttpPost]
        public IActionResult UpCreate(Category category)
        {
            var id =category.Id;

            if ( id == 0)
            {
                //Create
                _productContext.Categories.Add(category);
              
            }
            else
            {
                //Update

                _productContext.Categories.Update(category);
                
            }
            _productContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        //==================================== NEW ==========================================

        public IActionResult Delete(int id)
        {
            var category = _productContext.Categories.Find(id);
            if (category != null)
            {
                var product= _productContext.Products.Where(p => p.CategoryId == id).FirstOrDefault();
                if(product != null)
                {
                    TempData["message"] = "ไม่สามารถลบ เนื่องจากมีการใช้งานอยู่";
                    return RedirectToAction(nameof(Index));
                }

                _productContext.Categories.Remove(category);
                _productContext.SaveChangesAsync();
                TempData["message"] = "ลบสำเร็จ";
            }
             

            return RedirectToAction(nameof(Index));

        }
    }
}
