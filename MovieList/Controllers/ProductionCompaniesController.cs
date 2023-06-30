using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.DTO;
using MovieList.Models;
using System.Linq.Dynamic.Core;

namespace MovieList.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductionCompaniesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductionCompaniesController> _logger;

    public ProductionCompaniesController(
        ApplicationDbContext context,
        ILogger<ProductionCompaniesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet(Name = "GetProductionCompanies")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDTO<ProductionCompany[]>> Get(
        [FromQuery] RequestDTO<ProductionCompanyDTO> input)
    {
        // Search by Name
        var query = _context.ProductionCompanies.AsQueryable();
        if (!string.IsNullOrWhiteSpace(input.FilterQuery))
        {
            query = query.Where(m => m.Name.Contains(input.FilterQuery));
        }

        var recordCount = await query.CountAsync();

        query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);

        return new RestDTO<ProductionCompany[]>()
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
                        "ProductionCompanies",
                        new { input.PageIndex, input.PageSize },
                        Request.Scheme)!,
                    "self",
                    "GET"),
            }
        };
    }
        
}