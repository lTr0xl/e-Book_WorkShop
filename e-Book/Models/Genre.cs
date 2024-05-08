using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Book.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string GenreName { get; set; }

        public ICollection<BookGenre>? BookGenres { get; set; }
        public ICollection<Book>? Books { get; set; }

    }
}
