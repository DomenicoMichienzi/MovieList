using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieList.DTO;

public class MovieDTO
{
    [Required]
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public double? Popularity { get; set; }
    
    [DefaultValue(null)]
    public DateTime? ReleaseDate { get; set; } 
    
    public double? Revenue { get; set; }
    
    public string? Status { get; set; }
    
    public double? VoteAverage { get; set; }
    
    public double? VoteCount { get; set; }
}