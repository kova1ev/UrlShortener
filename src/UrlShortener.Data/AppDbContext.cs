using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    // public DbSet<Url> Urls => Set<Url>();
    //public DbSet<Url> Urls { get; set; }

    //public DbSet<Url> Urls => Set<Url>();

    DbSet<Url> IAppDbContext.Urls { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Url url = new Url("https://code-maze.com/aspnetcore-web-api-return-types/", "https://goo.su/banana") { Id = Guid.NewGuid() };

        modelBuilder.Entity<Url>().HasData(url);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        return base.SaveChangesAsync(cancellation);
    }
}


