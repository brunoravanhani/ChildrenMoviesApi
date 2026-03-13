using Microsoft.EntityFrameworkCore;
using ChildrenMoviesApi.Domain.Entity;

namespace ChildrenMoviesApi.Infrastructure.Data;

public class ChildrenMoviesDbContext : DbContext
{
    public ChildrenMoviesDbContext(DbContextOptions<ChildrenMoviesDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ConfigureMovieEntity(modelBuilder);
    }

    private static void ConfigureMovieEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.OriginalTitle).HasMaxLength(500);
            entity.Property(e => e.Overview).HasMaxLength(4000);
            entity.Property(e => e.OriginalLanguage).HasMaxLength(10);
            entity.Property(e => e.BackdropPath).HasMaxLength(500);
            entity.Property(e => e.PosterPath).HasMaxLength(500);
            entity.Property(e => e.ReleaseDate);
        });
    }
}
