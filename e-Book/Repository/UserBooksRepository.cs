using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<Book>> GetAllUserBooks()
        {
            var curUser = _httpContextAccessor.HttpContext?.User;
            List<UserBooks> userBooks =await _context.UserBooks.Where(u => u.AppUserId == curUser.ToString()).ToListAsync();
            List<Book> myBooks = new List<Book>();
            foreach(var book in userBooks)
            {
                myBooks.Add(await _context.Books.FirstOrDefaultAsync(i => i.Id == book.Id));
            }
            return myBooks;
        }
    }
}
