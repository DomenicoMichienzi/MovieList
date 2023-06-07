using System.ComponentModel.DataAnnotations;

namespace MovieList.Models
{
    public class Movies_Genres
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Key]
        [Required]
        public int GenreId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        public Movie? Movie { get; set; }
        public Genre? Genre { get; set; }
    }
}
