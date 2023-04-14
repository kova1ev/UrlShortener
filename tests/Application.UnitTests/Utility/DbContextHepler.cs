using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;

namespace Application.UnitTests.Utility;

public class DbContextHepler
{
    public static AppDbContext CreateContext()
    {
        DbContextOptionsBuilder<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseInMemoryDatabase(Guid.NewGuid().ToString());

        var context = new AppDbContext(options.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        SeedData.SeedInitData(context);

        return context;
    }
}
