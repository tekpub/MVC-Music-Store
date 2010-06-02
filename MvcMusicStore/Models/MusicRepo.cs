using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models {
    public class MusicRepo : MvcMusicStore.Models.IMusicRepo {
        public IQueryable<Album> Albums {
            get {
                return MusicStoreEntities.Current.Albums;               
            }
        }
        public IQueryable<Artist> Artists {
            get {
                return MusicStoreEntities.Current.Artists;
            }
        }
        public IQueryable<Cart> Carts {
            get {
                return MusicStoreEntities.Current.Carts;
            }
        }
        public IQueryable<Genre> Genres {
            get {
                return MusicStoreEntities.Current.Genres;
            }
        }
        public IQueryable<Order> Orders {
            get {
                return MusicStoreEntities.Current.Orders;
            }
        }

        public IQueryable<OrderDetail> OrderDetails {
            get {
                return MusicStoreEntities.Current.OrderDetails;
            }
        }
        public void Add<T>(T item) {
            MusicStoreEntities.Current.AddObject(GetSetName<T>(), item);
        }
        public void Delete<T>(T item) {
            MusicStoreEntities.Current.DeleteObject(item);
        }
        public void SaveChanges() {
            MusicStoreEntities.Current.SaveChanges();
        }

        string GetSetName<T>() {

            //If you get an error here it's because your namespace
            //for your EDM doesn't match the partial model class
            //to change - open the properties for the EDM FILE and change "Custom Tool Namespace"
            //Not - this IS NOT the Namespace setting in the EDM designer - that's for something
            //else entirely. This is for the EDMX file itself (r-click, properties)

            var entitySetProperty =
            MusicStoreEntities.Current.GetType().GetProperties()
               .Single(p => p.PropertyType.IsGenericType && typeof(IQueryable<>)
               .MakeGenericType(typeof(T)).IsAssignableFrom(p.PropertyType));

            return entitySetProperty.Name;
        }
    }
}