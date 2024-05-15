using e_Book.Models;

namespace e_Book.Interfaces
{
    public interface IUserBooksRepository
    {
        Task<List<Book>> GetAllUserBooks();
        string GetCurrentUserId();
        string GetCurrentUserUsername();
        bool HasBook(int bookId);
        Task<AppUser> GetUserById(string id);
        bool Add(UserBooks userBooks);
        bool Update(UserBooks userBooks);
        bool Delete(UserBooks userBooks);
        bool Save();
    }
}
