using System;
using CampingWebsiteAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace CampingWebsiteAPI.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _products = database.GetCollection<Product>("products");
        }

        public List<Product> Get() =>
            _products.Find(product => true).ToList();

        public Product Get(string id)
        {
            var allProductIds = _products.Find(product => true).ToList().Select(p => p.Id);
            Console.WriteLine("All Product Ids: " + string.Join(", ", allProductIds));

            var objectId = new ObjectId(id);
            var filter = Builders<Product>.Filter.Regex("_id", new BsonRegularExpression(objectId.ToString(), "i"));
            var product = _products.Find(filter).FirstOrDefault();

            if (product == null)
            {
                throw new NotFoundException($"Product not found with id {id}");
            }

            Console.WriteLine($"ProductService Get: {product.Id}, {product.Name}, {product.Price}");

            return product;
        }

        public IEnumerable<Product> SearchProducts(string searchTerm)
        {
            // Perform a case-insensitive search on the product's Name and Description
            var searchRegex = new BsonRegularExpression($"/{searchTerm}/i");

            var filter = Builders<Product>.Filter.Regex(x => x.Name, searchRegex) |
                        Builders<Product>.Filter.Regex(x => x.Description, searchRegex);

            return _products.Find(filter).ToList();
        }


        public List<Product> GetOnSaleProducts()
        {
            return _products.Find(product => product.IsOnSale).ToList();
        }

        public Dictionary<string, Product> GetAllAsDictionary()
        {
            var products = _products.Find(product => true).ToList();
            return products.ToDictionary(p => p.Id!, p => p);
        }

        public Product Create(Product product)
        {
            _products.InsertOne(product);
            return product;
        }

        public void Update(string id, Product productIn) =>
            _products.ReplaceOne(product => product.Id == id, productIn);

        public void Remove(Product productIn) =>
            _products.DeleteOne(product => product.Id == productIn.Id);

        public void Remove(string id) =>
            _products.DeleteOne(product => product.Id == id);
    }
}

