using CampingWebsiteMVC.Models;
using CampingWebsiteMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ProductController : Controller
{
    private readonly ICampingWebsiteApiService _apiService;

    public ProductController(ICampingWebsiteApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var response = await _apiService.GetAsync("api/Products");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductViewModel>>(content);
            return View(products);
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var response = await _apiService.GetAsync($"api/Products/{id}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductViewModel>(content);
            return View(product);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(CartItemViewModel cartItem)
    {
        var json = JsonConvert.SerializeObject(cartItem);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _apiService.PostAsync("api/Products/AddToCart", content);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Cart");
        }

        return BadRequest();
    }
}



