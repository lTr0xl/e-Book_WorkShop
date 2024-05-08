using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_Book.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Nationality { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? Gender { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
