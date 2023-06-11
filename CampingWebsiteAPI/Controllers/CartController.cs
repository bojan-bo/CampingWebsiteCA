using CampingWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace CampingWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;

        public CartController(CartService cartService, ProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [HttpPost("increase-quantity/{productId}")]
        public IActionResult IncreaseQuantity(string productId)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return Unauthorized();
            }

            _cartService.IncreaseQuantity(userId, productId);
            int cartItemCount = _cartService.GetCartsByUserId(userId).FirstOrDefault()?.Items.Sum(item => item.Quantity) ?? 0;

            return Ok(cartItemCount);
        }

        [HttpPost("decrease-quantity/{productId}")]
        public IActionResult DecreaseQuantity(string productId)
        {
            string userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                return Unauthorized();
            }

            _cartService.DecreaseQuantity(userId, productId);
            int cartItemCount = _cartService.GetCartsByUserId(userId).FirstOrDefault()?.Items.Sum(item => item.Quantity) ?? 0;

            return Ok(cartItemCount);
        }

        // Add other actions to manage the cart (e.g., AddItem, RemoveItem, UpdateQuantity)
    }
}



