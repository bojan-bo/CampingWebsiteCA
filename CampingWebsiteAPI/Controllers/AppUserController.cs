﻿using CampingWebsiteAPI.Models;
using CampingWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CampingWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserService _appUserService;
        private readonly IConfiguration _configuration;

        public AppUserController(AppUserService appUserService, IConfiguration configuration)
        {
            _appUserService = appUserService;
            _configuration = configuration;
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
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["Jwt:Key"];
                if (key != null)
                {
                    var encodedKey = Encoding.ASCII.GetBytes(key);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, appUser.Id.ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var response = new
                    {
                        UserId = appUser.Id,
                        UserName = appUser.Name,
                        CartItemCount = appUser.CartItems?.Count ?? 0, // Assuming CartItems is a list or collection
                        Token = tokenHandler.WriteToken(token)
                    };
                    return Ok(response);
                }
                else
                {
                    // Handle the case when the key is null, e.g., throw an exception or return an appropriate response
                    return StatusCode(500, "Jwt:Key is not configured.");
                }
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
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

