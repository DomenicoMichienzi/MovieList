using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieList.Models
{
    [Table("Genres")]
    public class Genre
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }

        public ICollection<Movies_Genres>? MoviesGenresCollection { get; set; }
    }
}
