using System;
namespace CampingWebsiteAPI.Models
{
    public class Cart
    {
        public string? UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}

