using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Book.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }
        public int? YearPublished { get; set; }
        public int? NumPages { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Publisher { get; set; }
        public string? FrontPage { get; set; }
        public string? DownloadUrl { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<BookGenre> BookGenres { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<UserBooks> UserBooks { get; set; }


    }
}
