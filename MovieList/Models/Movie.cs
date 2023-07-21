using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieList.Models;



[Table("Movies")]
public class Movie
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    [Required]
    public string OriginalLanguage { get; set; } = null!;

    [Required]
    public string Overview { get; set; } = null!;

    [Required]
    public double? Popularity { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public double? Budget { get; set; }

    [Required]
    public double? Revenue { get; set; }

    [Required]
    public double? Runtime { get; set; }

    [Required]
    public string? Status { get; set; } = null!;

    [Required]
    public double? VoteAverage { get; set; }

    [Required]
    public double? VoteCount { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime LastModifiedDate { get; set; }
    
    public ICollection<Movies_Genres>? MoviesGenresCollection { get; set; }
    public ICollection<Movies_ProductionCompanies>? MoviesProductionCompaniesCollection { get; set; }

}

