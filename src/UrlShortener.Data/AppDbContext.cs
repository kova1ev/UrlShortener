using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data.EntityConfiguration;
using UrlShortener.Entity;

namespace UrlShortener.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Link> Links { get; set; } = null!;
    public DbSet<LinkStatistic> LinkStatistics { get; set; } = null!;
    public DbSet<Geolocation> Geolocations { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        base.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.HasCollation("case_insensitivity", locale: "en-u-ks-level2", provider: "icu", deterministic: false);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LinkConfiguration());
        modelBuilder.ApplyConfiguration(new LinkStatisticConfiguration());
        modelBuilder.ApplyConfiguration(new GeolocationConfiguration());
    }
}