using Microsoft.EntityFrameworkCore;
using UrlShortener.Entity;

namespace UrlShortener.Application.Interfaces;


public interface IAppDbContext : IDisposable
{
    DbSet<Link> Links { get; set; }
    DbSet<LinkStatistic> LinkStatistics { get; set; }
    DbSet<Geolocation> Geolocations { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
    int SaveChanges();
}
