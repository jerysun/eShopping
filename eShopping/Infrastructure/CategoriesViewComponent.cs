using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly EShoppingContext _context;
        public CategoriesViewComponent(EShoppingContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories.OrderBy(c => c.Sorting).ToListAsync();
            return View(categories);
        }
    }
}
