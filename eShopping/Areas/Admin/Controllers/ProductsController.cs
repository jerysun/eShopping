using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly EShoppingContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

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

        //POST /admin/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var prod = await _context.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (prod != null)
                {
                    ModelState.AddModelError("", "The product already exists");
                    return View(product);
                }

                // Find it from internet or somewhere else and create dir wwwroot/media/products
                // Then copy/paste it to the dir. It's a default in case no image is uploaded. 
                string imageName = "noimage.png";

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                }
                product.Image = imageName;

                _context.Add(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been added.";
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
