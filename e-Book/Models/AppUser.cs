using Microsoft.AspNetCore.Identity;

namespace e_Book.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<UserBooks> UserBooks { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
