
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Data;


namespace EcommerceApp.Controllers
{
    // -------------------------------------------------------------------
    // Data Transfer Objects (DTOs)
    // -------------------------------------------------------------------

    /// <summary>
    /// Represents an item to be displayed in the cart view.
    /// </summary>
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty; // <-- FIX applied here
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }

    // -------------------------------------------------------------------
    // Cart Service Interface 
    // -------------------------------------------------------------------

    public interface ICartService
    {
        Task AddToCartAsync(int productId, int quantity = 1);
        Task<List<CartItemDto>> GetCartItemsAsync();
        Task ClearCartAsync();
    }

    // -------------------------------------------------------------------
    // Cart Service Implementation 
    // -------------------------------------------------------------------

    public class EfCoreCartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public EfCoreCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(int productId, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                
                return;
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                await _context.CartItems.AddAsync(newItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItemDto>> GetCartItemsAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Product) 
                .Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    Name = ci.Product.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity
                })
                .ToListAsync();
        }

        public async Task ClearCartAsync()
        {
            var itemsToRemove = await _context.CartItems.ToListAsync();
            _context.CartItems.RemoveRange(itemsToRemove);
            await _context.SaveChangesAsync();
        }
    }

    // -------------------------------------------------------------------
    // Cart MVC Controller 
    // -------------------------------------------------------------------

    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context; 

        public CartController(ICartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            // Get available products
            var availableProducts = await _context.Products.ToListAsync();
            ViewBag.Products = availableProducts;

            // Get cart items and pass them to the view
            var cartItems = await _cartService.GetCartItemsAsync();
            return View(cartItems);
        }

        // POST: /Cart/AddToCart/5 (Called from the shop page)
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            await _cartService.AddToCartAsync(id);
            // After adding, redirect to the Index action to show the updated cart
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}