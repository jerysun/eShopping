using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly EShoppingContext _context;

        public PagesController(EShoppingContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // IQueryable<Page> pages = from p in _context.Pages orderby p.Sorting select p;
            // IQueryable<Page> pages = _context.Pages.OrderBy(p => p.Sorting);
            // List<Page> pagesList = await pages.ToListAsync();
            List<Page> pagesList = await _context.Pages.OrderBy(p => p.Sorting).ToListAsync();
            return View(pagesList);
        }
    }
}