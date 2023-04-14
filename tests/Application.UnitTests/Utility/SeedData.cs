using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace Application.UnitTests.Utility;

public class SeedData
{

    public static void SeedInitData(AppDbContext context)
    {
        if (context.Links.Any() == false)
        {
            Geolocation geolocation1 = new()
            {
                Id = new Guid("E643C813-E760-4D7E-B5F5-B56223F0314E"),
            };
            Geolocation geolocation2 = new()
            {
                Id = new Guid("AA4A2354-0856-47F8-87E0-FC3C1DFBE0E5"),
            };
            context.AddRange(geolocation1, geolocation2);

            LinkStatistic linkStatistic1 = new()
            {
                Id = new Guid("1F083C27-6F99-46D7-8120-8299BFA579E1"),
                Geolocation = geolocation1,
                DomainName = "github.com"

            };
            LinkStatistic linkStatistic2 = new()
            {
                Id = new Guid("68321149-40A5-4DF0-925E-B224A8D5D861"),
                Geolocation = geolocation2,
                DomainName = "leetcode.com"
            };
            context.AddRange(linkStatistic1, linkStatistic2);

            Link link1 = new()
            {
                Id = new Guid("567BD1BF-6287-4331-A50E-82984DB0B97D"),
                LinkStatistic = linkStatistic1,
                Alias = "aaa",
                UrlAddress = "https://github.com/",
                UrlShort = "https://localhost:7072/r/aaa"
            };
            Link link2 = new()
            {
                Id = new Guid("64DE08F3-B627-46EE-AE23-C2C873FC4C11"),
                LinkStatistic = linkStatistic2,
                Alias = "bbb",
                UrlAddress = "https://leetcode.com/",
                UrlShort = "https://localhost:7072/r/bbb"
            };
            context.AddRange(link1, link2);
            context.SaveChanges();
        }

    }
}
