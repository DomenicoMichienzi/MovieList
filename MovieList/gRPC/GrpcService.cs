using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using MovieList.Models;

namespace MovieList.gRPC;

public class GrpcService : Grpc.GrpcBase
{
    private readonly ApplicationDbContext _context;

    public GrpcService(ApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task<MovieResponse> GetMovie(
        MovieRequest request, 
        ServerCallContext scc)
    {
        var m = await _context.Movies
            .Where(m => m.Id == request.Id)
            .FirstOrDefaultAsync();
        var response = new MovieResponse();
        if (m != null)
        {
            response.Id = m.Id;
            response.Title = m.Title;
            response.Year = m.ReleaseDate.Year;
            //response.ReleaseDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(m.ReleaseDate);
        }

        return response;
    }
}