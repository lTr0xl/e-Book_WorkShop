using e_Book.Interfaces;
using e_Book.Models;
using e_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Genre> genres = await _genreRepository.GetAll();
            return View(genres);
        }
        public IActionResult CreateGenre()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateGenre(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Failed to create genre!";
                return View(genre);
            }
            _genreRepository.Add(genre);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteGenre(int id)
        {
            IEnumerable<Genre> genres = await _genreRepository.GetAll();
            DeleteGenreViewModel genreVM = new DeleteGenreViewModel()
            {
                Genres=genres,
                genreId=id,
            };
            return View(genreVM);
        }
        public async Task<IActionResult> DeleteGenreConfirmed(int id)
        {
            Genre genre = await _genreRepository.GetByIdAsync(id);
            if(genre == null)
            {
                return View("Error");
            }
            _genreRepository.Delete(genre);
            return RedirectToAction("Index");
        }
    }
}
