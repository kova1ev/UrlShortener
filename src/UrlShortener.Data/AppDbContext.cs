using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data.EntityConfiguration;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Link> Links { get; set; }
    public DbSet<LinkStatistic> LinkStatistics { get; set; }
    public DbSet<Geolocation> Geolocations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        base.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation("case_insensitivity", locale: "en-u-ks-level2", provider: "icu", deterministic: false);

        modelBuilder.ApplyConfiguration(new LinkConfiguration());
        modelBuilder.ApplyConfiguration(new LinkStatisticConfiguration());
        modelBuilder.ApplyConfiguration(new GeolocationConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}


