using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using MovieList.gRPC;

namespace MovieList.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class GrpcController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<MovieResponse> GetMovie(int id)
    {
        using var channel = GrpcChannel
            .ForAddress("https://localhost:7156");
        var client = new gRPC.Grpc.GrpcClient(channel);
        var response = await client.GetMovieAsync(
            new MovieRequest { Id = id });
        return response;
    }
}