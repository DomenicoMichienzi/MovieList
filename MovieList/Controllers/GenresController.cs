using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.DTO;
using MovieList.Models;
using System.Linq.Dynamic.Core;
using MovieList.Attributes;

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
    [ResponseCache(
        CacheProfileName = "Any-60", 
        VaryByQueryKeys = new string[]{"*"})]
    [ManualValidationFilter]
    public async Task<ActionResult<RestDTO<Genre[]>>> Get(
        [FromQuery] RequestDTO<GenresDTO> input)
    {
        // Check ModelState Status
        if (!ModelState.IsValid)
        {
            var details = new ValidationProblemDetails(ModelState);

            details.Extensions["traceId"] = 
                    System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            
            if (ModelState.Keys.Any(k => k == "PageSize"))
            {
                details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.6.2";
                details.Status = StatusCodes.Status501NotImplemented;
                return new ObjectResult(details) {
                    StatusCode = StatusCodes.Status501NotImplemented
                };
            }
            else
            {
                details.Type =
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1";
                details.Status = StatusCodes.Status400BadRequest;
                return new BadRequestObjectResult(details);
            }
        }
        
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
    
    // TODO - POST Method
    
    // TODO - Delete Method
}