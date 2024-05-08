using e_Book.Data;
using e_Book.Interfaces;
using e_Book.Models;
using Microsoft.EntityFrameworkCore;

namespace e_Book.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Review review)
        {
            _context.Reviews.Add(review);
            return Save();
        }

        public bool Delete(Review review)
        {
            _context.Reviews.Remove(review);
            return Save();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool Update(Review review)
        {
            _context.Reviews.Update(review);
            return Save();
        }
    }
}
