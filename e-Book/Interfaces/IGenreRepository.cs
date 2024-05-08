using e_Book.Models;

namespace e_Book.Interfaces
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetByIdAsync(int id);
        bool Add(Genre genre);
        bool Update(Genre genre);
        bool Delete(Genre genre);
        bool Save();
    }
}
