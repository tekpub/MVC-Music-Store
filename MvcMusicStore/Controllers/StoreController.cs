using System.Linq;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        IMusicRepo _repo;
        public StoreController(IMusicRepo repo) {
            _repo = repo;
       }

        //
        // GET: /Store/

        public ActionResult Index()
        {
            // Retrieve list of Genres from database
            var genres = from genre in _repo.Genres
                         select genre.Name;

            // Set up our ViewModel
            var viewModel = new StoreIndexViewModel()
            {
                Genres = genres.ToList(),
                NumberOfGenres = genres.Count()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/Browse?Genre=Disco

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre from database
            var genreModel = _repo.Genres
                .Single(g => g.Name == genre);

            var viewModel = new StoreBrowseViewModel()
            {
                Genre = genreModel,
                Albums = _repo.Albums.Where(x=>x.GenreId == genreModel.GenreId).ToList()
            };

            return View(viewModel);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(int id)
        {
            var album = _repo.Albums
                .Single(a => a.AlbumId == id);

            return View(album);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = _repo.Genres.ToList();

            return View(genres);
        }
    }
}