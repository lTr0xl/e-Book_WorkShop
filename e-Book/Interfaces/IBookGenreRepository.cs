using e_Book.Models;

namespace e_Book.Interfaces
{
    public interface IBookGenreRepository
    {
        Task<IEnumerable<BookGenre>> GetAll();
        bool Add(BookGenre bookGenre);
        bool Update(BookGenre bookGenre);
        bool Delete(BookGenre bookGenre);
        bool Save();
    }
}
