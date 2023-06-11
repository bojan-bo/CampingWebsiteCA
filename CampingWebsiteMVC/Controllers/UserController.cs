using System.Text;
using CampingWebsiteMVC.Models;
using CampingWebsiteMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CampingWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ICampingWebsiteApiService _apiService;

        public UserController(ICampingWebsiteApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _apiService.PostAsync("/api/AppUser/register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _apiService.PostAsync("/api/AppUser/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserViewModel>(responseJson);

                // Store user information in session or cookie

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Perform any necessary logout actions

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Account(string userId)
        {
            var response = await _apiService.GetAsync($"/api/AppUser/account/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserViewModel>(responseJson);

                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(UserViewModel user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _apiService.PutAsync($"/api/AppUser/account/{user.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Account", new { userId = user.Id });
            }

            return View(user);
        }
    }

}

