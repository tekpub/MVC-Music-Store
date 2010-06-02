using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    [HandleError]
    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        IMusicRepo _repo;
        public StoreManagerController(IMusicRepo repo) {
            _repo = repo;
       }
        //
        // GET: /StoreManager/

        public ActionResult Index()
        {
            var albums = _repo.Albums
                .ToList();

            return View(albums);
        }

        // 
        // GET: /StoreManager/Create

        public ActionResult Create()
        {
            var viewModel = new StoreManagerViewModel
            {
                Album = new Album(),
                Genres = _repo.Genres.ToList(),
                Artists = _repo.Artists.ToList()
            };

            return View(viewModel);
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        [Transaction]
        public ActionResult Create(Album album)
        {
            try
            {
                //Save Album
                _repo.Add(album);

                return Redirect("/");
            }
            catch
            {
                //Invalid - redisplay with errors

                var viewModel = new StoreManagerViewModel
                {
                    Album = album,
                    Genres = _repo.Genres.ToList(),
                    Artists = _repo.Artists.ToList()
                };

                return View(viewModel);
            }
        }

        //
        // GET: /StoreManager/Edit/5

        public ActionResult Edit(int id)
        {
            var viewModel = new StoreManagerViewModel
            {
                Album = _repo.Albums.Single(a => a.AlbumId == id),
                Genres = _repo.Genres.ToList(),
                Artists = _repo.Artists.ToList()
            };

            return View(viewModel);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        [Transaction]
        public ActionResult Edit(int id, FormCollection formValues)
        {
            var album = _repo.Albums.Single(a => a.AlbumId == id);

            try
            {
                //Save Album

                UpdateModel(album, "Album");

                return RedirectToAction("Index");
            }
            catch
            {
                var viewModel = new StoreManagerViewModel
                {
                    Album = album,
                    Genres = _repo.Genres.ToList(),
                    Artists = _repo.Artists.ToList()
                };

                return View(viewModel);
            }
        }

        //
        // GET: /StoreManager/Delete/5

        public ActionResult Delete(int id)
        {
            var album = _repo.Albums.Single(a => a.AlbumId == id);

            return View(album);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost]
        [Transaction]
        public ActionResult Delete(int id, string confirmButton)
        {
            var album = _repo.Albums
                .Single(a => a.AlbumId == id);

            _repo.Delete(album);

            return View("Deleted");
        }
    }
}