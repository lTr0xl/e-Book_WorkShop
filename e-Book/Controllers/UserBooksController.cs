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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public UserBooksController(IUserBooksRepository userBooksRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _userBooksRepository = userBooksRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
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

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User?.GetUserId();
            var user = await _userBooksRepository.GetUserById(curUserId);
            if(user == null)
            {
                return View("Error");
            }
            var editUserProfileVM = new EditUserProfileViewModel()
            {
                Id = curUserId,
                Username = user.UserName,
                Password = user.PasswordHash,
            };
            return View(editUserProfileVM);
        }


    }
}
