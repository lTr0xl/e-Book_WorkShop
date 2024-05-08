using e_Book.Models;

namespace e_Book.ViewModels
{
    public class AuthorsViewModel
    {
        public IEnumerable<Author> Authors { get; set; }
        public string SearchByName { get; set; }
    }
}
