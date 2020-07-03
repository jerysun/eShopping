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

        // GET /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (cart.Count > 0)
            {
                CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();
                if (cartItem == null)
                {
                    cart.Add(new CartItem(product));
                }
                else
                {
                    ++cartItem.Quantity;
                }
            }
            else
            {
                cart.Add(new CartItem(product));
            }

            HttpContext.Session.SetJson("Cart", cart);
            return RedirectToAction("Index");
        }
    }
}
