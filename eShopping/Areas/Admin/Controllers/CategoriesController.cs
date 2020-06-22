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
    public class CategoriesController : Controller
    {
        private readonly EShoppingContext _context;

        public CategoriesController(EShoppingContext context)
        {
            _context = context;
        }

        // GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(c => c.Sorting).ToListAsync());
        }

        // GET /admin/categories/create
        public IActionResult Create() => View();

        // POST /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await _context.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);
                if (slug != null)
                {
                    //immediately give an error feedback to front end
                    ModelState.AddModelError("", "The category already exists");
                    return View(category); // re-fill the correct information
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The category has been added!";

                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category != null)
                return View(category);

            return NotFound();
        }

        // POST /admin/categories/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                var cat = await _context.Categories.Where(c => c.Id != id).FirstOrDefaultAsync(c => c.Slug == category.Slug);
                if (cat != null)
                {
                    ModelState.AddModelError("", "The category already exists!");
                    return View(category);
                }

                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The category has been edited.";

                //Actually here we only need id, but for generalization we use routeValues object
                return RedirectToAction("Edit", new { id }); // GET /admin/categories/edit/id
            }

            return View(category);
        }

        // DELETE /admin/pages/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exist any more!";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The category has been deleted.";
            }
            return RedirectToAction("Index");
        }

        // POST /admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1; // the first sorting number because the order of "home" category is zero

            foreach (var categoryId in id)
            {
                Category category = await _context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                _context.Update(category);
                await _context.SaveChangesAsync();
                ++count;
            }

            return Ok();
        }
    }
}
