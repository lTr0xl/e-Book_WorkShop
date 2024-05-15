using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace e_Book.Repository
{
    public class UserBooksRepository : IUserBooksRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserBooksRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User.GetUserId();
        }
        public string GetCurrentUserUsername()
        {
            var curUser = GetCurrentUserId();
            var username = _context.Users.FirstOrDefault(i => i.Id == curUser).UserName;
            return username;
        }
        public async Task<List<Book>> GetAllUserBooks()
        {
            var curUser = GetCurrentUserId();
            List<UserBooks> userBooks =await _context.UserBooks.Where(u => u.AppUserId == curUser).ToListAsync();
            List<Book> myBooks = new List<Book>();
            foreach(var book in userBooks)
            {
                var myBook = await _context.Books.Include(a => a.Author).Include(r => r.Reviews).Include(bg => bg.BookGenres).ThenInclude(g => g.Genre).FirstOrDefaultAsync(i => i.Id == book.BookId);
                myBooks.Add(myBook);
            }
            return myBooks;
        }


        public bool HasBook(int bookId)
        {
            var curUser = GetCurrentUserId();
            var hasBook = _context.UserBooks.FirstOrDefault(u => u.AppUserId == curUser && u.BookId == bookId);
            return hasBook != null ? true : false;
        }


        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }


        public bool Add(UserBooks userBooks)
        {
            _context.UserBooks.Add(userBooks);
            return Save();
        }

        public bool Update(UserBooks userBooks)
        {
            _context.UserBooks.Update(userBooks);
            return Save();
        }

        public bool Delete(UserBooks userBooks)
        {
            _context.UserBooks.Remove(userBooks);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


    }
}
