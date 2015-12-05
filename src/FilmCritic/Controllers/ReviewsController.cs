using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using FilmCritic.Models;
using Microsoft.AspNet.Authorization;
using System.Collections.Generic;
using System;

namespace FilmCritic.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly List<string> _unusedFilms;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        private IEnumerable<Review> GetMyReviews()
        {
            return _context.Review.ToList().Where(r => r.Name == User.Identity.Name);
        }

        // GET: Reviews
        public IActionResult Index()
        {
            return View(GetMyReviews());
        }

        // GET: Reviews/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Review review = _context.Review.Single(m => m.ID == id);
            if (review == null)
            {
                return HttpNotFound();
            }

            ViewData["ShortReview"] = ShortenReview(review.ReviewText);
            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            var TitleQry = from f in _context.Film
                           orderby f.Title
                           select f.Title;

            var TitleList = new List<string>(TitleQry);
            var unusedList = new List<string>();

            var myReviews = GetMyReviews();
            var count = 0;

            foreach (var title in TitleList)
            {
                foreach (var review in myReviews)
                {
                    if (title == review.FilmTitle)
                        count++;
                }

                if (count == 0)
                    unusedList.Add(title);

                count = 0;
            }

            ViewData["FilmList"] = new SelectList(unusedList);
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            [Bind("ID,Name,Title,ReviewText,FilmTitle")] string filmTitle, Review review)
        {
            review.FilmTitle = filmTitle;

            if (ModelState.IsValid)
            {
                _context.Review.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Review review = _context.Review.Single(m => m.ID == id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            [Bind("ID,Name,Title,ReviewText,FilmTitle")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Update(review);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Review review = _context.Review.Single(m => m.ID == id);
            if (review == null)
            {
                return HttpNotFound();
            }

            ViewData["ShortReview"] = ShortenReview(review.ReviewText);
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Review review = _context.Review.Single(m => m.ID == id);
            _context.Review.Remove(review);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private string ShortenReview(string review)
        {
            if (review.Length > 50)
                return review.Substring(0, 50) + "...";

            return review;
        }
    }
}
