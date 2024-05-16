using e_Book.Models;

namespace e_Book.Interfaces
{
    public interface IAccountRepository
    {
        string GetCurrentUserId();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
