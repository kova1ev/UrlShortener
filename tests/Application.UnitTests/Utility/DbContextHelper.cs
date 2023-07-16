using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UrlShortener.Data;

namespace Application.UnitTests.Utility;

public static class DbContextHelper
{
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        options.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        var context = new AppDbContext(options.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        SeedData.SeedInitData(context);

        return context;
    }
}