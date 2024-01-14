using Microsoft.AspNetCore.Mvc;
using ProductWeb.Services;

namespace ProductWeb.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;


        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        public async Task<IActionResult> Index()
        {
            var result = await _roleService.GetAll();
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(RoleDto roleDto)
        {
            var result = await _roleService.Add(roleDto);

            return RedirectToAction(nameof(Index));
        }


                                             //ส่งตัวที่จะแก้ (name)
        public async Task<IActionResult> Edit(string name)
        {
            var result = await _roleService.Find(name);


            var roleUpdate = new RoleUpdateDto { Name = result.Name };


            return View(roleUpdate);
        }

        //ส่งค่าที่ใส่ใหม่ ไปแสดง
        [HttpPost]
        public async Task<IActionResult> Edit(RoleUpdateDto roleUpdateDto)
        {
            var result = await _roleService.Update(roleUpdateDto);


            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string name)
        {
            var result = await _roleService.Delete(name);


            return RedirectToAction(nameof(Index));
        }


    }
}
