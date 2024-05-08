using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;

namespace e_Book.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Author author)
        {
            _context.Authors.Add(author);
            return Save();
        }

        public bool Delete(Author author)
        {
            _context.Authors.Remove(author);
            return Save();
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _context.Authors.Include(b => b.Books).ToListAsync();
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<IEnumerable<Author>> SearchByLastName(string lastName)
        {
            return await _context.Authors.Include(b => b.Books).Where(a => a.LastName.Contains(lastName)).ToListAsync();
        }

        public async Task<IEnumerable<Author>> SearchByName(string firstName)
        {
            return await _context.Authors.Include(b => b.Books).Where(a => a.FirstName.Contains(firstName)).ToListAsync();
        }

        public bool Update(Author author)
        {
            _context.Update(author);
            return Save();
        }
    }
}
