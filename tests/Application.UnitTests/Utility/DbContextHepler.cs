﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UrlShortener.Data;

namespace Application.UnitTests.Utility;

public class DbContextHepler
{
    public static AppDbContext CreateContext()
    {
        DbContextOptionsBuilder<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        options.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        var context = new AppDbContext(options.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        SeedData.SeedInitData(context);

        return context;
    }
}
