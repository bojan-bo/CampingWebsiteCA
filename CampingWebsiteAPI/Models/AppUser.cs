using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampingWebsiteAPI.Models
{
    public class AppUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("Email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("Password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("CartItems")]
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}

