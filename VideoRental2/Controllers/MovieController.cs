using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoRental2.Models;
using VideoRental2.ViewModels;
using System.Data.Entity;

namespace VideoRental2.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext _context; //to access database
        public MovieController()
        {
            _context = new ApplicationDbContext(); //initialize database variable
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose(); //dispose the database variable
        }
        public ActionResult Random()
        {
            //Having a movie and the associated customers renting it out, display that in view "Random"
            var movie = new Movie() { name = "Shrek" };
            var customers = new List<Customer>()
            {
                new Customer() {name = "Cust 1", id = 1 },
                new Customer() { name = "Cust 2", id = 2 }
            };
            var random = new RandomMovieViewModel() { randomMovie = movie, customers = customers };
            return View("Random",random);

            //return Content("This is straight up content");
            //return JavaScript("Callback()");  /unsuccesful in figureing out how this works

            //return files such as images and videos
            /*
            string mimeType = MimeMapping.GetMimeMapping("C:/Users/sanme_000/Documents/SeniorDesignVid-2017.mp4");
            var imagePath = "C:/Users/sanme_000/Documents/SeniorDesignVid-2017.mp4";
            return base.File(imagePath, mimeType);
            */

        }
        public ActionResult Other(int? pageNum, string sortBy) //put questionmark to signify optional parameter. strings in general are optional param so no need for ?
        {
            if (!pageNum.HasValue)
                pageNum = 0;
            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";
            return Content(String.Format("pageNum =  {0} and sortyBy = {1}", pageNum, sortBy));
        }

        public ActionResult CustomerList()
        {
            var customers = new List<Customer>()
            {
                new Customer() {name = "Sam", id = 1 },
                new Customer() { name = "Duncan", id = 2 }
            };
            return View("CustomerView",customers);

        }
        /*public List<Movie> moviesList = new List<Movie>()
            {
                new Movie() {name = "Rush Hour", id = 1 },
                new Movie() { name = "Dark Knight", id = 2 }
            };*/
        public ActionResult Index()
        {
            //Having a movie and the associated customers renting it out, display that in view "Random"
            var moviesList = _context.Movie.Include(c => c.Genre).ToList();
            return View(moviesList);

        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movie.Include(c => c.Genre).SingleOrDefault(c => c.id == id);
            if (movie == null)
                return HttpNotFound();
            return View(movie);

        }
        [Route("Movie/released/{year}/{month:regex(\\d{2}):range(01,12)}")]//atribute routing
        public ActionResult ByReleased(int year, int month)
        {
            return Content(String.Format("year = {0} month = {1}", year, month));
        }
        public ActionResult New()
        {
            //Having a movie and the associated customers renting it out, display that in view "Random"
            var viewModel = new NewMovieViewModel()
            {
                Genre = _context.Genre.ToList(),
               // Movie = new Movie() { id = 255,releaseDate = DateTime.Now }
            };
            return View(viewModel);

        }
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            //Having a movie and the associated customers renting it out, display that in view "Random"
            if (movie.id ==null)
            {
                if (_context.Movie.Count() == 0)
                {
                    movie.id = 1;
                }
                else
                    movie.id = (byte)(_context.Movie.Max(m => m.id) + 1);
                movie.dateAdded = DateTime.Now;
                _context.Movie.Add(movie);
            }
            else
            {
                var dbMovie = _context.Movie.Single(m => m.id == movie.id);
                dbMovie.name = movie.name;
                dbMovie.GenreId = movie.GenreId;
                dbMovie.releaseDate= movie.releaseDate;
                dbMovie.numberInStock = movie.numberInStock;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movie");
        }
        public ActionResult Edit(byte id)
        {
            var movie = _context.Movie.Include(c => c.Genre).SingleOrDefault(c => c.id == id);
            if (movie == null)
                return HttpNotFound();
            var viewModel = new NewMovieViewModel()
            {
                Genre = _context.Genre.ToList(),
                Movie = movie
            };
            return View("New", viewModel);

        }
    }
}