using e_Book.Models;

namespace e_Book.Interfaces
{
    public interface IUserBooksRepository
    {
        Task<List<Book>> GetAllUserBooks();
    }
}
