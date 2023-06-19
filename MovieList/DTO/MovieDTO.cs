using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieList.DTO;

public class MovieDTO
{
    [Required]
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public decimal? Popularity { get; set; }
    
    [DefaultValue(null)]
    public DateTime? ReleaseDate { get; set; } 
    
    public decimal? Revenue { get; set; }
    
    public string? Status { get; set; }
    
    public decimal? VoteAverage { get; set; }
    
    public decimal? VoteCount { get; set; }
}