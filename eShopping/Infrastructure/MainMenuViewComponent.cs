using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Infrastructure
{

    public class MainMenuViewComponent : ViewComponent
    {
        private readonly EShoppingContext _context;

        public MainMenuViewComponent(EShoppingContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await _context.Pages.OrderBy(p => p.Sorting).ToListAsync();

            return View(pages); //Views/Shared/Components/{ViewComponent}/Default.cshtml
        }
    }
}
