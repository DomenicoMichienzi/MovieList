﻿using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.Models;
using MovieList.Models.Csv;

namespace MovieList.Controllers;

[Route("[controller]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<SeedController> _logger;

    public SeedController(
        ApplicationDbContext context,
        IWebHostEnvironment env,
        ILogger<SeedController> logger)
    {
        _context = context;
        _env = env;
        _logger = logger;
    }

    [HttpPut(Name = "Seed")]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> Put()
    {
        // SETUP
        var config = new CsvConfiguration(CultureInfo.GetCultureInfo("en-US"))
        {
            HasHeaderRecord = true,
            Delimiter = ","
        };
        
        using var reader = new StreamReader(
            System.IO.Path.Combine(_env.ContentRootPath, "Data/movies_2016to2023.csv"));
        using var csv = new CsvReader(reader, config);
        var existingMovies = await _context.Movies
            .ToDictionaryAsync(m => m.Id);
        var existingGenres = await _context.Genres
            .ToDictionaryAsync(g => g.Name);
        var existingProductionCompanies = await _context.ProductionCompanies
            .ToDictionaryAsync(pc => pc.Name);
        var now = DateTime.Now;
        
        // EXECUTE
        var records = csv.GetRecords<MovieRecord>();
        var skippedRows = 0;
        foreach (var record in records)
        {
            if (!record.Id.HasValue
                || string.IsNullOrEmpty(record.Title)
                || existingMovies.ContainsKey(record.Id.Value))
            {
                skippedRows++;
                continue;
            }

            var movie = new Movie()
            {
                Id = record.Id.Value,
                Title = record.Title,
                OriginalLanguage = record.OriginalLanguage ?? string.Empty,
                Overview = record.Overview ?? string.Empty,
                Popularity = record.Popularity ?? 0,
                ReleaseDate = DateTime.ParseExact(record.ReleaseDate!, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Budget = record.Budget ?? 0,
                Revenue = record.Revenue ?? 0,
                Runtime = record.Runtime ?? 0,
                Status = record.Status,
                VoteAverage = record.VoteAverage ?? 0,
                VoteCount = record.VoteCount ?? 0,
                CreatedDate = now,
                LastModifiedDate = now
            };

            _context.Add(movie);

            // Genres
            if (!string.IsNullOrEmpty(record.Genres))
            {
                foreach (var genreName in record.Genres
                             .Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                             .Distinct(StringComparer.InvariantCulture))
                {
                    var genre = existingGenres.GetValueOrDefault(genreName);
                    if (genre == null)
                    {
                        genre = new Genre()
                        {
                            Name = genreName,
                            CreatedTime = now,
                            LastModifiedDate = now
                        };
                        _context.Genres.Add(genre);
                        existingGenres.Add(genreName, genre);
                    }

                    _context.Movies_Genres.Add(new Movies_Genres()
                    {
                        Movie = movie,
                        Genre = genre,
                        CreatedTime = now
                    });
                }
            }
            
            // ProductionCompanies
            if (!string.IsNullOrEmpty(record.ProductionCompanies))
            {
                foreach (var prodCompsName in record.ProductionCompanies
                             .Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                             .Distinct(StringComparer.InvariantCulture))
                {
                    var prodComps = existingProductionCompanies.GetValueOrDefault(prodCompsName);
                    if ( prodComps == null)
                    {
                        prodComps = new ProductionCompany()
                        {
                            Name = prodCompsName,
                            CreatedTime = now,
                            LastModifiedDate = now
                        };
                        _context.ProductionCompanies.Add(prodComps);
                        existingProductionCompanies.Add(prodCompsName, prodComps);
                    }

                    _context.Movies_ProductionCompanies.Add(new Movies_ProductionCompanies()
                    {
                        Movie = movie,
                        ProductionCompany = prodComps,
                        CreatedTime = now
                    });
                }
            }
        }
        
        // SAVE
        using var transaction = _context.Database.BeginTransaction();
        await _context.SaveChangesAsync();
        transaction.Commit();

        // RECAP
        return new JsonResult(new
        {
            Movies = _context.Movies.Count(),
            Genres = _context.Genres.Count(),
            ProductionCompanies = _context.ProductionCompanies.Count(),
            SkippedRows = skippedRows
        });
    }
}