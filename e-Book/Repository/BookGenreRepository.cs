using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;

namespace e_Book.Repository
{
    public class BookGenreRepository : IBookGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public BookGenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(BookGenre bookGenre)
        {
            _context.BookGenres.Add(bookGenre);
            return Save();
        }

        public bool Delete(BookGenre bookGenre)
        {
            _context.BookGenres.Remove(bookGenre);
            return Save();
        }

        public async Task<IEnumerable<BookGenre>> GetAll()
        {
            return await _context.BookGenres.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(BookGenre bookGenre)
        {
            _context.BookGenres.Update(bookGenre);
            return Save();
        }
    }
}
