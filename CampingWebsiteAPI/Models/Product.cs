using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CampingWebsiteAPI.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("category_id")]
        public string CategoryId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image_url")]
        public string ImageUrl { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }


        [BsonElement("IsOnSale")]
        public bool IsOnSale { get; set; }

        [BsonElement("OriginalPrice")]
        public double OriginalPrice { get; set; }

    }
}

