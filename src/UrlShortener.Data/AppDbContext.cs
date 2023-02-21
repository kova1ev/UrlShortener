using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Link> Links { get; set; }
    public DbSet<LinkInfo> LinkInfos { get; set; }

    public override DatabaseFacade Database => base.Database;


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        base.Database.EnsureCreated();
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

}


