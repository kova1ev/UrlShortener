using Microsoft.EntityFrameworkCore;
using UrlShortener.Data.EntityConfiguration;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data;

public class AppDbContext : DbContext
{
    public DbSet<Link> Links { get; set; }
    public DbSet<LinkInfo> LinkInfos { get; set; }

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
        modelBuilder.ApplyConfiguration(new LinkConfiguration());
        modelBuilder.ApplyConfiguration(new LinkInfoConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}


