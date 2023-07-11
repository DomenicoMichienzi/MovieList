using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieList.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Movies_Genres Fluent API
            modelBuilder.Entity<Movies_Genres>()
                .HasKey(i => new { i.MovieId, i.GenreId });

            modelBuilder.Entity<Movies_Genres>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MoviesGenresCollection)
                .HasForeignKey(x => x.MovieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movies_Genres>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.MoviesGenresCollection)
                .HasForeignKey(x => x.GenreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Movies_ProductionCompanies Fluent API
            modelBuilder.Entity<Movies_ProductionCompanies>()
                .HasKey(i => new { i.MovieId, i.ProductionCompanyId });

            modelBuilder.Entity<Movies_ProductionCompanies>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.MoviesProductionCompaniesCollection)
                .HasForeignKey(x => x.MovieId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Movies_ProductionCompanies>()
                .HasOne(x => x.ProductionCompany)
                .WithMany(x => x.MoviesProductionCompaniesCollection)
                .HasForeignKey(x => x.ProductionCompanyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<ProductionCompany> ProductionCompanies => Set<ProductionCompany>();
        public DbSet<Movies_Genres> Movies_Genres => Set<Movies_Genres>();
        public DbSet<Movies_ProductionCompanies> Movies_ProductionCompanies => Set<Movies_ProductionCompanies>();
    }
}
