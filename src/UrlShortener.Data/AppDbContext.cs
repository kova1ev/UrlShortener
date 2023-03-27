using Microsoft.EntityFrameworkCore;
using UrlShortener.Data.EntityConfiguration;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data;

public class AppDbContext : DbContext
{
    public DbSet<Link> Links { get; set; }
    public DbSet<LinkStatistic> LinkStatistics { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        base.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCamelCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation("case_insensitivity", locale: "en-u-ks-level2", provider: "icu", deterministic: false);

        modelBuilder.ApplyConfiguration(new LinkConfiguration());
        modelBuilder.ApplyConfiguration(new LinkStatisticConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}


