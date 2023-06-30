using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.DTO;
using MovieList.Models;
using System.Linq.Dynamic.Core;

namespace MovieList.Controllers;

[Route("[controller]")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GenresController> _logger;

    public GenresController(
        ApplicationDbContext context,
        ILogger<GenresController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet(Name = "GetGenres")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDTO<Genre[]>> Get(
        [FromQuery] RequestDTO<GenresDTO> input)
    {
        // Search by Name
        var query = _context.Genres.AsQueryable();
        if (!string.IsNullOrWhiteSpace(input.FilterQuery))
        {
            query = query.Where(m => m.Name.Contains(input.FilterQuery));
        }

        var recordCount = await query.CountAsync();

        query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);

        return new RestDTO<Genre[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "Genres",
                        new { input.PageIndex, input.PageSize },
                        Request.Scheme)!,
                    "self",
                    "GET"),
            }
        };
    }
}