using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly EShoppingContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(EShoppingContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int p = 1)
        {
            const int pageSize = 6;
            // Include is just 'join' in SQL. Every time we return a product, it must include the associated category
            var products = await _context.Products.OrderByDescending(p => p.Id)
                                                  .Include(p => p.Category)
                                                  .Skip((p - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();

            const int pageRange = 10;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageRange;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

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
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Sorting), "Id", "Name");

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
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
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

        public async Task<IActionResult> Details(int id)
        {
            Product product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                return View(product);
            }

            return NotFound();
        }

        // GET /admin/products/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Sorting).ToList(), "Id", "Name", product.CategoryId);
                return View(product);
            }
            return NotFound();

        }

        // POST /admin/products/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Sorting).ToList(), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                var p = await _context.Products.Where(p => p.Id != id).FirstOrDefaultAsync(p => p.Slug == product.Slug);

                if (p != null)
                {
                    ModelState.AddModelError("", "The product already exists!");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    if (!string.Equals(product.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, product.Image);
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                    product.Image = imageName;
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been updated.";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
                TempData["Error"] = "The product does not exist!";
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string fileDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string filePath = Path.Combine(fileDir, product.Image);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}
























