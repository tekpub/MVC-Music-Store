using System;
namespace MvcMusicStore.Models {
    public interface IMusicRepo {
        void Add<T>(T item);
        System.Linq.IQueryable<Album> Albums { get; }
        System.Linq.IQueryable<Artist> Artists { get; }
        System.Linq.IQueryable<Cart> Carts { get; }
        void Delete<T>(T item);
        System.Linq.IQueryable<Genre> Genres { get; }
        System.Linq.IQueryable<OrderDetail> OrderDetails { get; }
        System.Linq.IQueryable<Order> Orders { get; }
        void SaveChanges();
    }
}
