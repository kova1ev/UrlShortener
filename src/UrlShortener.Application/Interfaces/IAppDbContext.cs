using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Interfaces;


public interface IAppDbContext
{
    DbSet<Link> Links { get; set; }
    DbSet<LinkStatistic> LinkStatistics { get; set; }
    DbSet<Geolocation> Geolocations { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
