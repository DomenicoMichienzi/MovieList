using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.DTO;
using MovieList.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using MovieList.Constants;

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
    [ResponseCache(
        CacheProfileName = "Any-60", 
        VaryByQueryKeys = new string[]{"*"})]
    public async Task<RestDTO<ProductionCompany[]>> Get(
        [FromQuery] RequestDTO<ProductionCompanyDTO> input)
    {
        // Search by Name
        var query = _context.ProductionCompanies.AsQueryable();
        if (!string.IsNullOrWhiteSpace(input.FilterQuery))
        {
            query = query.Where(
                m => m.Name
                    .ToLower() // ToLower() instead of ToLowerInvariant() for trouble with EF
                    .Contains(input.FilterQuery.ToLowerInvariant())
                );
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
    
    [Authorize(Roles = RoleNames.Moderator)]
    [HttpPost(Name = "UpdateProductionCompany")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<RestDTO<ProductionCompany?>> Post(ProductionCompanyDTO model)
    {
        var productionCompany = await _context.ProductionCompanies
            .Where(x => x.Id == model.Id)
            .FirstOrDefaultAsync();

        if (productionCompany != null)
        {
            if (!string.IsNullOrWhiteSpace(model.Name))
                productionCompany.Name = model.Name;

            productionCompany.LastModifiedDate = DateTime.Now;
            _context.ProductionCompanies.Update(productionCompany);
            await _context.SaveChangesAsync();
        }
        
        return new RestDTO<ProductionCompany?>()
        {
            Data = productionCompany,
            Links = new List<LinkDTO>()
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "ProductionCompanies",
                        model,
                        Request.Scheme)!,
                    "self",
                    "POST"),
            }
        };
    }
    
    [Authorize(Roles = RoleNames.Moderator)]
    [HttpDelete(Name = "DeleteProductionCompany")]
    [ResponseCache(CacheProfileName = "NoCache")]
    public async Task<RestDTO<ProductionCompany?>> Delete(int id)
    {
        var productionCompany = await _context.ProductionCompanies
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (productionCompany != null)
        {
            _context.ProductionCompanies.Remove(productionCompany);
            await _context.SaveChangesAsync();
        }
        
        return new RestDTO<ProductionCompany?>()
        {
            Data = productionCompany,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "ProductionCompanies",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE"),
            }
        };

    }
}