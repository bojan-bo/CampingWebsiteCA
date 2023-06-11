using System.Net;
using CampingWebsiteMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampingWebsiteMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICampingWebsiteApiService _apiService;

        public CartController(ICampingWebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> IncreaseQuantity(string productId)
        {
            // Get the user ID from your authentication system
            string userId = "UserId";

            var response = await _apiService.PostAsync($"Cart/increase-quantity/{productId}", null);
            if (response.IsSuccessStatusCode)
            {
                var cartItemCount = await response.Content.ReadAsStringAsync();
                return Ok(int.Parse(cartItemCount));
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                // Handle other status codes
                return StatusCode((int)response.StatusCode);
            }
        }

        public async Task<IActionResult> DecreaseQuantity(string productId)
        {
            // Get the user ID from your authentication system
            string userId = "user-id";

            var response = await _apiService.PostAsync($"Cart/decrease-quantity/{productId}", null);
            if (response.IsSuccessStatusCode)
            {
                var cartItemCount = await response.Content.ReadAsStringAsync();
                return Ok(int.Parse(cartItemCount));
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            else
            {
                // Handle other status codes
                return StatusCode((int)response.StatusCode);
            }
        }
    }

}


