using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //GET /admin/roles
        public IActionResult Index() => View(_roleManager.Roles);

        //GET /admin/roles/create
        public IActionResult Create() => View();

        //POST /admin/roles/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([MinLength(2), Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["Success"] = "The role has been created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            ModelState.AddModelError("", "Minimum length is 2");
            return View();
        }

    }
}





















