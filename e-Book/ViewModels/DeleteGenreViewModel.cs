using e_Book.Models;

namespace e_Book.ViewModels
{
    public class DeleteGenreViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        public int genreId { get; set; }
    }
}
