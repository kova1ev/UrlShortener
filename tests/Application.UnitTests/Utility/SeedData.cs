using UrlShortener.Data;
using UrlShortener.Entity;

namespace Application.UnitTests.Utility;

public class SeedData
{
    public static List<Link> Links => new List<Link>
    {
        new Link()
        {
            Id = new Guid("567BD1BF-6287-4331-A50E-82984DB0B97D"),
            Alias = "aaa",
            UrlAddress = "https://github.com",
            UrlShort = "https://localhost:7072/r/aaa",
            LinkStatistic = new LinkStatistic
            {
                Id = new Guid("1F083C27-6F99-46D7-8120-8299BFA579E1"),
                DomainName = "github.com",
                Geolocation = new Geolocation
                {
                    Id = new Guid("E643C813-E760-4D7E-B5F5-B56223F0314E")
                }
            }
        },
        new Link()
        {
            Id = new Guid("64DE08F3-B627-46EE-AE23-C2C873FC4C11"),
            Alias = "bbb",
            UrlAddress = "https://leetcode.com",
            UrlShort = "https://localhost:7072/r/bbb",
            LinkStatistic = new LinkStatistic
            {
                Id = new Guid("68321149-40A5-4DF0-925E-B224A8D5D861"),
                DomainName = "leetcode.com",
                Clicks = 10,
                Geolocation = new Geolocation
                {
                    Id = new Guid("AA4A2354-0856-47F8-87E0-FC3C1DFBE0E5")
                }
            }
        },
    };
    

    public static void SeedInitData(AppDbContext context)
    {
        if (context.Links.Any() == false)
        {
            context.AddRange(Links);
            context.SaveChanges();
        }
    }
}