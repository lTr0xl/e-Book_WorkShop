using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
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
            List<Book> myBooks = await _userBooksRepository.GetAllUserBooks();
            return View(myBooks);
        }
    }
}
