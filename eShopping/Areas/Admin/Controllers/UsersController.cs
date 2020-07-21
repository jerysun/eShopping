using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Areas.Admin
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
    }
}
