using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.LinkStatistics.Commands;

public class UpdateLinkStatisticCommandHandler : IRequestHandler<UpdateLinkStatisticCommand, Result>
{
    // private readonly IHttpContextAccessor _httpContextAccessor;

    //  private readonly IServiceScopeFactory _serviceScopeFactory;
    // private readonly IAppDbContext appDbContext;
    private readonly ISystemDateTime _systemDateTime;

    public UpdateLinkStatisticCommandHandler(ISystemDateTime systemDateTime)
    {
        Console.WriteLine("-> ctor update ");
        _systemDateTime = systemDateTime;
    }
    // public UpdateLinkStatisticCommandHandler(ISystemDateTime systemDateTime,IAppDbContext appDbContext)
    // {
    //     _systemDateTime = systemDateTime;
    //     this.appDbContext = appDbContext;
    // }

    public async Task<Result> Handle(UpdateLinkStatisticCommand request, CancellationToken cancellationToken)
    {
        var id = Thread.CurrentThread.ManagedThreadId;
        Random rnd = new Random();
        Console.WriteLine("--> before try in thread id {0}", id);
        try
        {
            Console.WriteLine(" ---->start try thread id {0}", id);
            using (var scope = request.ServiceScopeFactory.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                Console.WriteLine(" ----->get db context");
                // await Task.Delay(TimeSpan.FromSeconds(4), cancellationToken);
                var linkStatistic = appDbContext.LinkStatistics
                    .Include(st => st.Geolocation)
                    .FirstOrDefault(s => s.Id == request.Id);
                int sec = rnd.Next(10, 21);
                Console.WriteLine("---> waiting time {0}", sec);
                if (sec == 10 || sec == 20)
                {
                    throw new InvalidOperationException("sec > 12 ");
                }
                await Task.Delay(TimeSpan.FromSeconds(sec));
                // if (linkStatistic == null)
                //   return Result.Failure(new string[] { LinkStatisticsErrorMessage.NotFound });

                linkStatistic.Browser = request.AgentInfo.Browser;
                linkStatistic.Os = request.AgentInfo.Os;
                linkStatistic.Clicks++;
                linkStatistic.LastUse = _systemDateTime.UtcNow;

                linkStatistic.Geolocation!.Country = request.Geolocation.Country;
                linkStatistic.Geolocation.Region = request.Geolocation.Region;
                linkStatistic.Geolocation.City = request.Geolocation.City;

                appDbContext.LinkStatistics.Update(linkStatistic);
                appDbContext.SaveChanges();
            }

            Console.WriteLine("*************************************** ");

            return Result.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}