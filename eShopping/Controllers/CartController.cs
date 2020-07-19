using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopping.Infrastructure;
using eShopping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Controllers
{
    [Authorize]
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

            // judge if it's an AJAX request
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");
        }


        // GET /cart/decrease/5
        public IActionResult Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            if (cart == null)
                return NotFound();

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();
            if (cartItem == null)
                return NotFound();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else if (cartItem.Quantity == 1)
            {
                cart.Remove(cartItem);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        // GET /cart/remove/5
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            if (cart == null)
                return NotFound();

            CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();
            if (cartItem == null)
                return NotFound();

            cart.Remove(cartItem);
            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }

        // GET /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            // judge if it's an ajax request
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }
    }
}
