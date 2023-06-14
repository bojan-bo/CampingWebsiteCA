using CampingWebsiteAPI.Models;
using CampingWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CampingWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly AppUserService _appUserService;

        public ProductsController(ProductService productService, CartService cartService, AppUserService appUserService)
        {
            _productService = productService;
            _cartService = cartService;
            _appUserService = appUserService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productService.Get();
            if (products.Count == 0)
            {
                return NotFound("Products not available");
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var product = _productService.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToCart([FromBody] CartItem cartItem)
        {
            string userId = HttpContext.User.Identity.Name;

            _cartService.AddItem(userId, cartItem);

            AppUser currentUser = _appUserService.GetUserById(userId);
            currentUser.CartItems = _cartService.GetCartsByUserId(userId).FirstOrDefault()?.Items;
            _appUserService.Update(currentUser.Id, currentUser);

            return Ok();
        }
    }
}
