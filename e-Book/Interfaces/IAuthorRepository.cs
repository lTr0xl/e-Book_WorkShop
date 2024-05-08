using e_Book.Models;

namespace e_Book.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAll();
        Task<Author> GetByIdAsync(int id);
        Task<IEnumerable<Author>> SearchByName(string firstName);
        Task<IEnumerable<Author>> SearchByLastName(string lastName);
        bool Add(Author author);
        bool Update(Author author);
        bool Delete(Author author);
        bool Save();
    }
}
