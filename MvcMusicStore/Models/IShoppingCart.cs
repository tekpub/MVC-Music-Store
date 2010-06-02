using System;
namespace MvcMusicStore.Models {
    public interface IShoppingCart {
        void AddToCart(Album album, string cartid);
        int CreateOrder(Order order, string cartid);
        void EmptyCart(string cartid);
        System.Collections.Generic.List<Cart> GetCartItems(string cartid);
        int GetCount(string cartid);
        decimal GetTotal(string cartid);
        void MigrateCart(string fromCartId, string userName);
        void RemoveFromCart(int id, string cartid);
    }
}
