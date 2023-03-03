using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Interfaces;

//undone
public interface IAppDbContext
{
    DbSet<Link> Links { get; set; }
    DbSet<LinkInfo> LinkInfos { get; set; }

    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
