using e_Book.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAll();
        Task<Book> GetByIdAsync(int id);
        Task<Book> GetByIdAsyncNoTracking(int id);
        Task<Book> GetLastBook();
        Task<IEnumerable<Book>> GetBooksByName(string name);
        Task<IEnumerable<Author>> GetAllAuthors();
        Task<IEnumerable<Genre>> GetAllGenres();
        Task<IEnumerable<Book>> GetBooksByAuthorId(int id);
        Task<IEnumerable<Book>> GetBooksByGenreId(int id);
        bool Add(Book book);
        bool Update(Book book);
        bool Delete(Book book);
        bool Save();
    }
}
