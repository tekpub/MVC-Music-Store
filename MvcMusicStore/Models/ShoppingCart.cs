using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public partial class ShoppingCart:IShoppingCart
    {
        IMusicRepo _repo;
        public ShoppingCart(IMusicRepo repo) {
            _repo = repo;
        }

        public void AddToCart(Album album, string cartid)
        {
            var cartItem = _repo.Carts.SingleOrDefault(
                c => c.CartId == cartid && 
                c.AlbumId == album.AlbumId);

            if (cartItem == null)
            {
                // Create a new cart item
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = cartid,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _repo.Add(cartItem);
            }
            else
            {
                // Add one to the quantity
                cartItem.Count++;
            }
        }

        public void RemoveFromCart(int id, string cartid)
        {
            //Get the cart
            var cartItem = _repo.Carts.Single(
                cart => cart.CartId == cartid 
                && cart.RecordId == id);

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                }
                else
                {
                    _repo.Delete(cartItem);
                }
            }
        }

        public void EmptyCart(string cartid)
        {
            var cartItems = _repo.Carts
                .Where(cart => cart.CartId == cartid);

            foreach (var cartItem in cartItems)
            {
                _repo.Delete(cartItem);
            }

        }

        public List<Cart> GetCartItems(string cartid)
        {
            var cartItems = (from cart in _repo.Carts
                             where cart.CartId == cartid
                             select cart).ToList();
            return cartItems;
        }

        public int GetCount(string cartid)
        {
            int? count = (from cartItems in _repo.Carts
                          where cartItems.CartId == cartid
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        public decimal GetTotal(string cartid)
        {
            decimal? total = 
                (from cartItems in _repo.Carts
                where cartItems.CartId == cartid
                select (int?)cartItems.Count * cartItems.Album.Price)
                .Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order, string cartid)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems(cartid);

            //Iterate the items in the cart, adding Order Details for each
            foreach (var cartItem in cartItems)
            {
                var orderDetails = new OrderDetail
                {
                    AlbumId = cartItem.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = cartItem.Album.Price
                };

                _repo.Add(orderDetails);

                orderTotal += (cartItem.Count * cartItem.Album.Price);
            }

 
            //Empty the shopping cart
            EmptyCart(cartid);

            //Return the OrderId as a confirmation number
            return order.OrderId;
        }


        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string fromCartId, string userName)
        {
            var shoppingCart = _repo.Carts
                .Where(c => c.CartId == fromCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
         }
    }
}