using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using e_Book.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Controllers
{
    public class UserBooksController : Controller
    {
        private readonly IUserBooksRepository _userBooksRepository;

        public UserBooksController(IUserBooksRepository userBooksRepository)
        {
            _userBooksRepository = userBooksRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Book> myBooks = await _userBooksRepository.GetAllUserBooks();
            return View(myBooks);
        }
        public async Task<IActionResult> BuyBook(int id)
        {
            var curUser = _userBooksRepository.GetCurrentUserId();
            UserBooks newUB = new UserBooks()
            {
                AppUserId = curUser,
                BookId = id,
            };
            _userBooksRepository.Add(newUB);
            return RedirectToAction("Index");
        }

    }
}
