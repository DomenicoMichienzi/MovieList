using MovieList.Models;

namespace MovieList.GraphQL;

public class Query
{
    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Movie> GetMovies(
        [Service] ApplicationDbContext context)
        => context.Movies;
    
    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductionCompany> GetProductionCompanies(
        [Service] ApplicationDbContext context)
        => context.ProductionCompanies;
    
    [Serial]
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Genre> GetGenres(
        [Service] ApplicationDbContext context)
        => context.Genres;
}