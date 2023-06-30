using System.ComponentModel.DataAnnotations;

namespace MovieList.DTO;

public class GenresDTO
{
    [Required]
    public int Id { get; set; }
    
    public string? Name { get; set; }
}