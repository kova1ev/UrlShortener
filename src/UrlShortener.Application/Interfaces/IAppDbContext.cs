using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Interfaces;
public interface IAppDbContext
{
    DbSet<Url> Urls { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellation);
}
