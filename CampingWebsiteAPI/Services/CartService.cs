// Services/CartService.cs
using CampingWebsiteAPI.Models;
using System.Collections.Generic;
using System.Linq;
using CampingWebsiteAPI.Services;
using MongoDB.Driver;


namespace CampingWebsiteAPI.Services
{
    public class CartService
    {
        private readonly AppUserService _appUserService;

        public CartService(AppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public void IncreaseQuantity(string userId, string productId)
        {
            var user = _appUserService.GetUserById(userId);
            var item = user.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                // If the item is not in the cart, add it
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = 1
                };
                user.CartItems.Add(newItem);
            }
            _appUserService.Update(user.Id, user);
        }


        public async Task ClearCartAsync(string userId)
        {
            // Get the current user
            var currentUser = _appUserService.GetUserById(userId);

            // Clear the cart items
            currentUser.CartItems.Clear();

            // Update the user in the database
            _appUserService.Update(currentUser.Id, currentUser);
        }






        public void DecreaseQuantity(string userId, string productId)
        {
            var user = _appUserService.GetUserById(userId);
            var item = user.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    user.CartItems.Remove(item);
                }
                _appUserService.Update(user.Id, user);
            }
        }

        public Task<int> GetCartItemCountAsync(string userId)
        {
            var currentUser = _appUserService.GetUserById(userId);
            return Task.FromResult(currentUser.CartItems.Count);
        }

        public List<Cart> GetCartsByUserId(string userId)
        {
            var currentUser = _appUserService.GetUserById(userId);
            return new List<Cart> { new Cart { UserId = userId, Items = currentUser.CartItems } };
        }

        public void AddItem(string userId, CartItem cartItem)
        {
            // Get the current user
            var currentUser = _appUserService.GetUserById(userId);

            // Find the user's cart
            var cart = currentUser.CartItems;

            // Check if the product is already in the cart
            var existingItem = cart.FirstOrDefault(i => i.ProductId == cartItem.ProductId);

            if (existingItem != null)
            {
                // If the product is already in the cart, update its quantity
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                // Add the new item to the cart
                currentUser.CartItems.Add(cartItem);
            }

            // Update the user in the database
            _appUserService.Update(currentUser.Id, currentUser);
        }

        // Update other methods (UpdateItem, RemoveItem) in a similar manner


        public void UpdateItem(string userId, CartItem cartItem)
        {
            // Get the current user
            var currentUser = _appUserService.GetUserById(userId);

            // Find the existing cart item
            var existingItem = currentUser.CartItems.FirstOrDefault(i => i.ProductId == cartItem.ProductId);

            if (existingItem != null)
            {
                // Update the cart item's quantity
                existingItem.Quantity = cartItem.Quantity;
            }

            // Update the user in the database
            _appUserService.Update(currentUser.Id, currentUser);
        }

        public void RemoveItem(string userId, string productId)
        {
            // Get the current user
            var currentUser = _appUserService.GetUserById(userId);

            // Remove the cart item with the specified product ID
            currentUser.CartItems.RemoveAll(i => i.ProductId == productId);

            // Update the user in the database
            _appUserService.Update(currentUser.Id, currentUser);
        }



        // Add other methods to manage the cart (e.g., AddItem, RemoveItem, UpdateQuantity)
    }
}

