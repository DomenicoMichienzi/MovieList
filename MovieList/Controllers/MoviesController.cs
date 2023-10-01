using DefaultValueAttribute = System.ComponentModel.DefaultValueAttribute;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.DTO;
using MovieList.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using MovieList.Attributes;
using MovieList.Constants;

namespace MovieList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoviesController> _logger;
        
        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetMovies")]
        [ResponseCache(
            CacheProfileName = "Any-60", 
            VaryByQueryKeys = new string[]{"*"})]
        public async Task<RestDTO<Movie[]>> Get(
            [DefaultValue(0)] int pageIndex = 0,
            [Range(1, 100)] int pageSize = 10,
            [SortColumnValidator(typeof(MovieDTO))] string? sortColumn = "Title",
            [RegularExpression("ASC|DESC")] string? sortOrder = "ASC",
            string? filterQuery = null
            )
        {
            _logger.LogInformation(CustomLogEvents.MoviesController_Get,
                "Get method started.");
            
            var query = _context.Movies.AsQueryable();
            
            // Search by Title
            if (!string.IsNullOrWhiteSpace(filterQuery))
            {   
                // use ToLowerInvariant on both Title and filterQuery
                query = query.Where(
                    m => m.Title
                    .ToLower() // ToLower() instead of ToLowerInvariant() for trouble with EF
                    .Contains(filterQuery.ToLowerInvariant()));

            }

            var recordCount = await query.CountAsync();

            query = query
                .OrderBy($"{sortColumn} {sortOrder}")
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            
            return new RestDTO<Movie[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO>{
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Movies",
                            new { pageIndex, pageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [Authorize]
        [HttpPost(Name = "UpdateMovie")]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<RestDTO<Movie?>> Post(MovieDTO model)
        {
            var movie = await _context.Movies
                .Where(m => m.Id == model.Id)
                .FirstOrDefaultAsync();
            
            if (movie != null)
            {
                if (!string.IsNullOrWhiteSpace(model.Title))
                    movie.Title = model.Title;
                if (model.ReleaseDate.HasValue)
                    movie.ReleaseDate = model.ReleaseDate.Value;

                movie.LastModifiedDate = DateTime.Now;

                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Movie?>()
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Movies",
                            model,
                            Request.Scheme)!,
                        "self",
                        "POST"),
                }
            };
        }
    
        [Authorize(Roles = RoleNames.Moderator)]
        [HttpDelete(Name = "DeleteMovie")]
        [ResponseCache(CacheProfileName = "NoCache")]
        public async Task<RestDTO<Movie?>> Delete(int id)
        {
            var movie = await _context.Movies
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }

            return new RestDTO<Movie?>()
            {
                Data = movie,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Movies",
                            id,
                            Request.Scheme)!,
                        "self",
                        "DELETE"),
                }
            };
        }
    }
}
