using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Interfaces;


public interface IAppDbContext
{
    DbSet<Link> Links { get; set; }
    DbSet<LinkStatistic> LinkStatistics { get; set; }
    DbSet<Geolocation> Geolocations { get; set; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
