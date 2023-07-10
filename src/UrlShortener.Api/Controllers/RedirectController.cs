using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Infrastructure;
using UrlShortener.Api.Utility;
using UrlShortener.Application.Common.Domain;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Links.Queries.GetLinkByShortName;
using UrlShortener.Application.LinkStatistics.Commands;
using UrlShortener.Application.Services;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("/r")]
public class RedirectController : ApiControllerBase
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private readonly IGeolocationService _geolocationService;
    private readonly ILogger<RedirectController> _logger;

    public RedirectController(IMediator mediator, IGeolocationService geolocationService,
        IConfiguration configuration, ILogger<RedirectController> logger, IServiceScopeFactory serviceScopeFactory)
        : base(mediator)
    {
        _geolocationService = geolocationService;
        _configuration = configuration;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    [HttpGet("{alias}")]
    public async Task<IActionResult> RedirectTo([FromRoute] string alias)
    {
        var result = await Mediator.Send(new GetLinkByShortNameQuery(alias));
        if (result.IsSuccess == false)
            return RedirectToPage("/NotFound");

        // todo move to facade 
        var clientIp = HttpContext.Request.GetClientIp();
#if DEBUG
        if (clientIp == null)
            clientIp = _configuration.GetValue<string>("ApiAddress");
#endif

        // TODO 1-2 sec server latency => make background work 
        // todo add add canceled 1-2 sec?
        Geolocation geolocation = new();
        if (clientIp != null)
        {
            geolocation = await _geolocationService.GetData(clientIp);
        }

        var userAgentInfo = HttpContext.Request.TryGetUserAgentInfo();

        // Task.Run(() => new TaskWorker().Execute(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id,
        //    userAgentInfo,
        //   geolocation, _serviceScopeFactory))).ConfigureAwait(false);
        Task.Run(() => Mediator.Send(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id, userAgentInfo,
            geolocation, _serviceScopeFactory))).ConfigureAwait(false);
        // var resultUpdate =
        // await Mediator.Send(new UpdateLinkStatisticCommand(result.Value.LinkStatistic.Id, userAgentInfo,
        // geolocation));
        // if (resultUpdate.IsSuccess == false)
        // {
        //     _logger.LogError("Link statistics not updated id: {@Id} Message: {@ErrorMessage}", result.Value.Id,
        //         resultUpdate.Errors?.First());
        // }
        return Redirect(result.Value.UrlAddress!);
    }
}

public class TaskWorker
{
    private readonly ISystemDateTime _systemDateTime = new SystemDateTime();

    public async Task Execute(UpdateLinkStatisticCommand request)
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
                Console.WriteLine(" ----->get db context thread id {0}", id);
                int sec = rnd.Next(10, 21);
                Console.WriteLine("---> waiting time {0}", sec);
                if (sec == 10 || sec == 20)
                {
                    throw new InvalidOperationException("sec > 12 ");
                }

                await Task.Delay(TimeSpan.FromSeconds(sec));
                var linkStatistic = appDbContext.LinkStatistics
                    .Include(st => st.Geolocation)
                    .FirstOrDefault(s => s.Id == request.Id);

                if (linkStatistic == null)
                    throw new InvalidOperationException();

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

            Console.WriteLine("*************************************** thread id {0}", id);
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + e.Message);
            Console.ResetColor();
            //throw;
        }
    }
}