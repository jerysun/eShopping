using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Controllers
{
    public class CartController : Controller
    {
        private readonly EShoppingContext _context;
        public CartController(EShoppingContext context)
        {
            _context = context;
        }

        // GET /cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(c => c.Total)
            };

            return View(cartVM);
        }
    }
}
