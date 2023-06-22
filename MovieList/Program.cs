using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieList.Models;
using MovieList.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(cfg => {
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]);
        cfg.AllowAnyHeader();
        cfg.AllowAnyMethod();
    });
    options.AddPolicy(name: "AnyOrigin",
        cfg => {
            cfg.AllowAnyOrigin();
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ParameterFilter<SortColumnFilter>();
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("WebApiMovieDatabase"))
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
    app.UseDeveloperExceptionPage();
else app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

// Minimal API
app.MapGet("/error",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => Results.Problem());

app.MapGet("/error/test",
    [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)]
    () => { throw new Exception("test"); });

// Controllers
app.MapControllers().RequireCors("AnyOrigin");

app.Run();
