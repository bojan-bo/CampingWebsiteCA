using CampingWebsiteAPI.Models;
using CampingWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CampingWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserService _appUserService;

        public AppUserController(AppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpPost("register")]
        public IActionResult Register(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                appUser.CreatedAt = DateTime.UtcNow;
                // Hash the password
                appUser.Password = BCrypt.Net.BCrypt.HashPassword(appUser.Password);
                _appUserService.Create(appUser);
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestModel model)
        {
            var appUser = _appUserService.FindByEmail(model.Email);
            if (appUser != null && BCrypt.Net.BCrypt.Verify(model.Password, appUser.Password))
            {
                var response = new
                {
                    UserId = appUser.Id,
                    UserName = appUser.Name,
                    CartItemCount = appUser.CartItems.Sum(item => item.Quantity)
                };
                return Ok(response);
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Perform any necessary logout actions
            return Ok();
        }

        [HttpGet("account/{userId}")]
        public IActionResult GetAccount(string userId)
        {
            var appUser = _appUserService.GetUserById(userId);
            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(appUser);
        }

        [HttpPut("account/{userId}")]
        public IActionResult UpdateAccount(string userId, AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _appUserService.Update(userId, appUser);
                return Ok();
            }

            return BadRequest(ModelState);
        }
    }

    public class LoginRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

