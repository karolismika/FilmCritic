using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using FilmCritic.Models;
using Microsoft.AspNet.Http.Internal;
using System.Collections.Generic;

namespace FilmCritic.Controllers
{
    public class FilmsController : Controller
    {
        private ApplicationDbContext _context;

        public FilmsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Films
        public IActionResult Index(string filmGenre, string titleSearchString, string nameSearchString)
        {
            var GenreQry = from m in _context.Film
                           orderby m.Genre
                           select m.Genre;

            var GenreList = new List<string>();
            GenreList.AddRange(GenreQry.Distinct());
            ViewData["GenreList"] = new SelectList(GenreList);
            
            var films = from f in _context.Film
                         select f;

            if (!string.IsNullOrEmpty(titleSearchString))
            {
                films = films.Where(ts => ts.Title.Contains(titleSearchString));
            }

            if (!string.IsNullOrEmpty(nameSearchString))
            {
                films = films.Where(ns => ns.Name.Contains(nameSearchString));
            }

            if (!string.IsNullOrEmpty(filmGenre))
            {
                films = films.Where(x => x.Genre == filmGenre);
            }

            return View(films);
        }

        [HttpPost]
        public string Index(FormCollection fc, string searchString)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("ID,Name,Title,Genre,Link")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Film.Add(film);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }

        public IActionResult Reviews(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Film film = _context.Film.Single(m => m.ID == id);
            if (film == null)
            {
                return HttpNotFound();
            }

            var ReviewQry = from r in _context.Review
                                     orderby r.FilmTitle
                                     where r.FilmTitle == film.Title
                                     select r;

            var filmReviews = new List<Review>(ReviewQry);

            return View(filmReviews);
        }
    }
}
