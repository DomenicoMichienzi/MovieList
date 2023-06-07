using System.ComponentModel.DataAnnotations;

namespace MovieList.Models
{
    public class Movies_ProductionCompanies
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Key]
        [Required]
        public int ProductionCompanyId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        public Movie? Movie { get; set; }
        public ProductionCompany? ProductionCompany { get; set; }
    }
}
