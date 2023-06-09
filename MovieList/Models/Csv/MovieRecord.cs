using CsvHelper.Configuration.Attributes;

namespace MovieList.Models.Csv;

// id	title	genres	original_language	overview
// popularity	production_companies	release_date
// budget	revenue	runtime	status	tagline	vote_average
// vote_count	credits	keywords	poster_path	backdrop_path 

public class MovieRecord
{
    [Name("id")]
    public int? Id { get; set; }
    
    [Name("title")]
    public string? Title { get; set; }
    
    [Name("genres")]
    public string? Genres { get; set; }
    
    [Name("original_language")]
    public string? OriginalLanguage { get; set; }
    
    [Name("overview")]
    public string? Overview { get; set; } = null!;
    
    [Name("popularity")]
    public decimal? Popularity { get; set; }
    
    [Name("production_companies")]
    public string? ProductionCompanies { get; set; }
    
    [Name("release_date")]
    public string? ReleaseDate { get; set; }
    
    [Name("budget")]
    public decimal? Budget { get; set; }
    
    [Name("revenue")]
    public decimal? Revenue { get; set; }
    
    [Name("runtime")]
    public decimal? Runtime { get; set; }
    
    [Name("status")]
    public string? Status { get; set; }
    
    [Name("tagline")]
    public string? Tagline { get; set; }
    
    [Name("vote_average")]
    public decimal? VoteAverage { get; set; }
    
    [Name("vote_count")]
    public decimal? VoteCount { get; set; }
    
    [Name("credits")]
    public string? Credits { get; set; }
    
    [Name("keywords")]
    public string? Keywords { get; set; }
    
    [Name("poster_path")]
    public string? PosterPath { get; set; }
    
    [Name("backdrop_path")]
    public string? BackdropPath { get; set; }
}