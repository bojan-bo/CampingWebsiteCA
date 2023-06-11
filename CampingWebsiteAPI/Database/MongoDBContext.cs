using MongoDB.Driver;

namespace CampingWebsiteAPI.Database
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;

        public MongoDBContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetSection("MongoDBSettings:ConnectionString").Value);
            _database = client.GetDatabase(configuration.GetSection("MongoDBSettings:DatabaseName").Value);
        }


    }
}

