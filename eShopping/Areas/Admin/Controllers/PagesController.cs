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

        // GET /admin/pages
        public async Task<IActionResult> Index()
        {
            // IQueryable<Page> pages = from p in _context.Pages orderby p.Sorting select p;
            // IQueryable<Page> pages = _context.Pages.OrderBy(p => p.Sorting);
            // List<Page> pagesList = await pages.ToListAsync();
            List<Page> pagesList = await _context.Pages.OrderBy(p => p.Sorting).ToListAsync();
            return View(pagesList);
        }

        // GET /admin/pages/details/5
        public async Task<IActionResult> Details(int id)
        {
            Page page = await _context.Pages.FirstOrDefaultAsync(p => p.Id == id);

            if (page != null)
                return View(page);

            return NotFound();
        }

        // GET /admin/pages/create
        public IActionResult Create() => View();

        // POST /admin/pages/create
        [HttpPost]
        public async Task<IActionResult> Create(Page page) // Model binding
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(' ', '-');
                page.Sorting = 100;

                var slug = await _context.Pages.FirstOrDefaultAsync(p => p.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The title already exists");
                    return View(page); // re-fill the correct information
                }

                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(page);  // re-fill the correct information
        }
    }
}