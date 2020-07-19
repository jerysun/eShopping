using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Controllers
{
    [Authorize]
    public class PagesController : Controller
    {
        private readonly EShoppingContext _context;

        public PagesController(EShoppingContext context)
        {
            _context = context;
        }


        // GET / or /slug
        public async Task<IActionResult> Page(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return View(await _context.Pages.Where(p => p.Slug == "home").FirstOrDefaultAsync());
            }

            Page page = await _context.Pages.Where(p => p.Slug == slug).FirstOrDefaultAsync();
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
