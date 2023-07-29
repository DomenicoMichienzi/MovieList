using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieList.Constants;
using MovieList.DTO;
using MovieList.Models;

namespace MovieList.GraphQL;

public class Mutation
{
    [Serial]
    [Authorize(Roles = new[] { RoleNames.Moderator })]
    public async Task<Movie?> UpdateMovie(
        [Service] ApplicationDbContext context, MovieDTO model)
    {
        var movie = await context.Movies
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        
        if (movie == null)
            return movie;
        
        // Title
        if (!string.IsNullOrEmpty(model.Title))
            movie.Title = model.Title;
        // Status
        if (!string.IsNullOrWhiteSpace(model.Status))
            movie.Status = model.Status;
         
        movie.LastModifiedDate = DateTime.Now;
        context.Movies.Update(movie);
        await context.SaveChangesAsync();
        
        return movie;
    }
}