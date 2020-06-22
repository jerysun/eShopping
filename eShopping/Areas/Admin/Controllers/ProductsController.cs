using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        public readonly EShoppingContext _context;

        public ProductsController(EShoppingContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Include is just 'join' in SQL. Every time we return a product, it must include the associated category
            List<Product> products = await _context.Products.OrderByDescending(p => p.Id).Include(p => p.Category).ToListAsync();
            return View(products);
        }

        //GET /admin/products/create
        public IActionResult Create()
        {
            // options, option value, option text
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Sorting).ToList(), "Id", "Name");
            return View();
        }
    }
}
