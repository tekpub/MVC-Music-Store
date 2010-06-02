using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System;

namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        IMusicRepo _repo;
        IShoppingCart _cart;
        string _cartid;
        public ShoppingCartController(IMusicRepo repo, IShoppingCart cart) {
            _repo = repo;
            _cart = cart;
            _cartid = MvcApplication.GetCartId();
        }
        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = _cart.GetCartItems(_cartid),
                CartTotal = _cart.GetTotal(_cartid)
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5
        [Transaction]
        public ActionResult AddToCart(int id)
        {

            // Retrieve the album from the database
            var addedAlbum = _repo.Albums
                .Single(album => album.AlbumId == id);

            _cart.AddToCart(addedAlbum, _cartid);
            
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        [Transaction]
        public ActionResult RemoveFromCart(int id)
        {
            // Get the name of the album to display confirmation
            string albumName = _repo.Carts
                .Single(item => item.RecordId == id).Album.Title;

            // Remove from cart. Note that for simplicity, we're 
            // removing all rather than decrementing the count.
            _cart.RemoveFromCart(id, _cartid);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel { 
                Message = Server.HtmlEncode(albumName) + 
                    " has been removed from your shopping cart.",
                CartTotal = _cart.GetTotal(_cartid),
                CartCount = _cart.GetCount(_cartid),
                DeleteId = id 
            };

            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {

            ViewData["CartCount"] = _cart.GetCount(_cartid);

            return PartialView("CartSummary");
        }


    }
}