using System.ComponentModel.DataAnnotations;

namespace MovieList.DTO;

public class ProductionCompanyDTO
{
    [Required]
    public int Id { get; set; }
    
    public string? Name { get; set; }
}