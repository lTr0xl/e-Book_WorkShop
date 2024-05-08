using e_Book.Interfaces;
using e_Book.Models;
using e_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<IActionResult> Index(AuthorsViewModel authorsVM)
        {
            IEnumerable<Author> authors;
            if(authorsVM.SearchByName == null)
            {
                authors = await _authorRepository.GetAll();
            }
            else
            {
                var nameLast = authorsVM.SearchByName.Split(' ');
                IEnumerable<Author> authorN = await _authorRepository.SearchByName(nameLast[0]);
                IEnumerable<Author> authorL = null;
                if (nameLast.Length > 1)
                {
                    authorL = await _authorRepository.SearchByLastName(nameLast[1]);
                    authors = authorN.Concat(authorL);
                }
                else
                {
                    if (authorN.Count() == 0)
                    {
                        authorL = await _authorRepository.SearchByLastName(nameLast[0]);
                        authors = authorL;
                    }
                    else
                    {
                        authors = authorN;
                    }
                }

                authors = authors.DistinctBy(i => i.Id);
            }
            AuthorsViewModel viewModel = new AuthorsViewModel()
            {
                Authors = authors,
            };
            return View(viewModel);
        }
        public IActionResult CreateAuthor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to create author");
                return View(author);
            }
            _authorRepository.Add(author);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            Author author = await _authorRepository.GetByIdAsync(id);
            if(author == null) 
            {
                return View("Error");
            }
            return View(author);
        }
        [HttpPost,ActionName("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthorConfirmed(int id)
        {
            Author author = await _authorRepository.GetByIdAsync(id);
            if(author == null)
            {
                return View("Error");
            }
            _authorRepository.Delete(author);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditAuthor(int id)
        {
            Author author = await _authorRepository.GetByIdAsync(id);
            if(author == null)
            {
                return View("Error");
            }
            return View(author);
        }
        [HttpPost]
        public async Task<IActionResult> EditAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit author");
                return View(author);
            }
            _authorRepository.Update(author);
            return RedirectToAction("Index");
        }
        
    }
}
