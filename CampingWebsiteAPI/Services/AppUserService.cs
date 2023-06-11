using System;
using CampingWebsiteAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace CampingWebsiteAPI.Services
{
    public class AppUserService
    {
        private readonly IMongoCollection<AppUser> _appUsers;
        private readonly ProductService _productService;


        public AppUserService(IMongoDBSettings settings, ProductService productService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _appUsers = database.GetCollection<AppUser>("appUsers");
            _productService = productService;
        }



        public List<AppUser> Get() =>
            _appUsers.Find(appUser => true).ToList();

        public AppUser Get(string id) =>
            _appUsers.Find<AppUser>(appUser => appUser.Id == id).FirstOrDefault();

        public AppUser Create(AppUser appUser)
        {
            _appUsers.InsertOne(appUser);
            return appUser;
        }

        public void Update(string id, AppUser appUserIn)
        {
            _appUsers.ReplaceOne(appUser => appUser.Id == id, appUserIn);
        }

        public void Remove(AppUser appUserIn) =>
            _appUsers.DeleteOne(appUser => appUser.Id == appUserIn.Id);

        public void Remove(string id) =>
            _appUsers.DeleteOne(appUser => appUser.Id == id);

        public AppUser FindByEmailAndPassword(string email, string password) =>
            _appUsers.Find<AppUser>(appUser => appUser.Email == email && appUser.Password == password).FirstOrDefault();

        public AppUser FindByEmail(string email)
        {
            return _appUsers.Find<AppUser>(appUser => appUser.Email == email).FirstOrDefault();
        }

        public void Update(AppUser appUser)
        {
            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, appUser.Id);
            var update = Builders<AppUser>.Update
                .Set(u => u.Name, appUser.Name)
                .Set(u => u.Password, appUser.Password);
            _appUsers.UpdateOne(filter, update);
        }

        // Add the GetUserById method
        public AppUser GetUserById(string id)
        {
            return _appUsers.Find<AppUser>(user => user.Id == id).FirstOrDefault();
        }
    }
}

