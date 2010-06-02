using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Mvc;
using Ninject.Modules;
using System.Reflection;
using System.Data.Objects;
using MvcMusicStore.Models;

namespace MvcMusicStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : NinjectHttpApplication
    {
        const string CartSessionKey = "CartId";
        // We're using HttpContextBase to allow access to cookies.
        public static string GetCartId() {
            var result = Guid.NewGuid().ToString();
            if (HttpContext.Current != null) {
                var context = HttpContext.Current;
                if (context.Session[CartSessionKey] == null) {
                    if (!string.IsNullOrWhiteSpace(context.User.Identity.Name)) {
                        // User is logged in, associate the cart with there username
                        result = context.User.Identity.Name;
                    }
                    //store in the session
                    context.Session[CartSessionKey] = result;
                } else {
                    result = context.Session[CartSessionKey].ToString();
                }
            }
            return result.ToString();
        }
        protected override void OnApplicationStarted() {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            RegisterAllControllersIn(Assembly.GetExecutingAssembly());
        }
        static IKernel _container;
        public static IKernel Container {
            get {
                if (_container == null) {
                    _container = new StandardKernel(new SiteModule());
                }
                return _container;
            }
        }
        protected override IKernel CreateKernel() {
            return Container;
        }

        internal class SiteModule : NinjectModule {
            public override void Load() {
                Bind<ObjectContext>().To<MusicStoreEntities>().InRequestScope();
                Bind<IMusicRepo>().To<MusicRepo>().InRequestScope();
                Bind<IShoppingCart>().To<ShoppingCart>().InRequestScope();
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Browse",                                                // Route name
                "Store/Browse/{genre}",                                  // URL with parameters
                new { controller = "Store", action = "Browse", id = "" } // Parameter defaults
            );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

    }
}