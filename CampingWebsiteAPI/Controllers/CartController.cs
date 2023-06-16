﻿using CampingWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CampingWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
            string? userId = HttpContext.User?.Identity?.Name;
            if (userId != null)
            {
                _cartService.IncreaseQuantity(userId, productId);
                int cartItemCount = _cartService.GetCartsByUserId(userId).FirstOrDefault()?.Items.Sum(item => item.Quantity) ?? 0;

                return Ok(cartItemCount);
            }

            return Unauthorized();
        }

        [HttpPost("decrease-quantity/{productId}")]
        public IActionResult DecreaseQuantity(string productId)
        {
            if (HttpContext.User?.Identity?.Name != null)
            {
                string userId = HttpContext.User.Identity.Name;
                _cartService.DecreaseQuantity(userId, productId);
                int cartItemCount = _cartService.GetCartsByUserId(userId).FirstOrDefault()?.Items.Sum(item => item.Quantity) ?? 0;

                return Ok(cartItemCount);
            }

            return Unauthorized();
        }
    }
}

