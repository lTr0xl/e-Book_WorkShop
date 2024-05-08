using AutoMapper;
using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace e_Book.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Book book)
        {
            _context.Books.Add(book);
            return Save();
        }

        public bool Delete(Book book)
        {
            _context.Books.Remove(book);
            return Save();
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await _context.Books.Include(a => a.Author).Include(r => r.Reviews).Include(bg => bg.BookGenres).ThenInclude(g => g.Genre).ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorId(int id)
        {
            return await _context.Books.Include(a => a.Author).Include(r => r.Reviews).Include(bg => bg.BookGenres).ThenInclude(g => g.Genre).Where(a => a.AuthorId.Equals(id)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreId(int id)
        {
            List<BookGenre> bookGenres = await _context.BookGenres.Where(g => g.GenreId.Equals(id)).ToListAsync();
            List<Book> books = new List<Book>();
            foreach(var bg in bookGenres)
            {
                Book book = await GetByIdAsync(bg.BookId);
                books.Add(book);
            }
            return books;
        }

        public async Task<IEnumerable<Book>> GetBooksByName(string name)
        {
            return await _context.Books.Include(a => a.Author).Include(r => r.Reviews).Include(bg => bg.BookGenres).ThenInclude(g => g.Genre).Where(n => n.Title.Contains(name)).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.Include(a => a.Author).Include(r => r.Reviews).Include(bg => bg.BookGenres).ThenInclude(g => g.Genre).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Book> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Books.Include(a => a.Author).Include(r => r.Reviews).Include(bg => bg.BookGenres).ThenInclude(g => g.Genre).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Book> GetLastBook()
        {
            return await _context.Books.FirstOrDefaultAsync(i => i.Id == _context.Books.Max(i => i.Id));
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Book book)
        {
            /*if (book != null)
            {
                ShowEntityState(_context);
                _context.Entry(book).State = EntityState.Modified;
                ShowEntityState(_context);
            }*/
            /*var trackedReference = _context.Books.Local.SingleOrDefault(i => i.Id == book.Id);
            if (trackedReference == null)
            {
                _context.Books.Update(book);
            }
            else if (!Object.ReferenceEquals(trackedReference, book))
            {
                Mapper.ReferenceEquals(book, trackedReference);
            }*/
            _context.Books.Update(book);
            return Save();
        }
        
    }
}
