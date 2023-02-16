using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Interfaces;
public interface IAppDbContext
{
    DbSet<Link> Links { get; set; }
    //DbSet<LinkInfo> LinkInfos { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellation = default);
}
