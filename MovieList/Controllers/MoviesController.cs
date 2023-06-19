using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.DTO;
using MovieList.Models;
using System.Linq.Dynamic.Core;

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
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
        public async Task<RestDTO<Movie[]>> Get(
            int pageIndex = 0,
            int pageSize = 25,
            string? sortColumn = "Title",
            string? sortOrder = "ASC",
            string? filterQuery = null)
        {
            var query = _context.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterQuery))
                query = query.Where(m => m.Title.Contains(filterQuery));

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
                Links = new List<LinkDTO> {
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

        [HttpPost(Name = "UpdateMovie")]
        [ResponseCache(NoStore = true)]
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

        [HttpDelete(Name = "DeleteMovie")]
        [ResponseCache(NoStore = true)]
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
