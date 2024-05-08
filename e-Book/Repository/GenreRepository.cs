using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;

namespace e_Book.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Genre genre)
        {
            _context.Genres.Add(genre);
            return Save();
        }

        public bool Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
            return Save();  
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.Include(bg => bg.BookGenres).ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Genre genre)
        {
            _context.Genres.Update(genre);
            return Save();
        }
    }
}
