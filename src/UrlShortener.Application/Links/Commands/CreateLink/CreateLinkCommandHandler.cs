using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Result<LinkCreatedResponse>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly ILinkService _linkService;

    public CreateLinkCommandHandler(IAppDbContext appDbContext, ILinkService aliasService)
    {
        this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _linkService = aliasService ?? throw new ArgumentNullException(nameof(aliasService));
    }

    public async Task<Result<LinkCreatedResponse>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        if (request.Alias != null && await _linkService.AliasIsBusy(request.Alias))
        {
            return Result<LinkCreatedResponse>.Failure(new string[] { LinkValidationErrorMessage.ALIAS_TAKEN });
        }

        if (request.Alias == null)
        {
            LinkCreatedResponse? existingLink = await _appDbContext.Links
                .AsNoTracking()
                .Where(l => l.UrlAddress == request.UrlAddress)
                .Select(l => new LinkCreatedResponse(l.Id, l.UrlShort))
                .FirstOrDefaultAsync();

            if (existingLink != null)
            {
                return Result<LinkCreatedResponse>.Success(existingLink);
            }
        }

        string alias = request.Alias ?? await _linkService.GenerateAlias();

        Link link = new Link()
        {
            UrlAddress = request.UrlAddress!,
            Alias = alias,
            UrlShort = _linkService.CreateShortUrl(alias)
        };
        await _appDbContext.Links.AddAsync(link);

        LinkStatistic linkStatistic = new LinkStatistic()
        {
            DomainName = new Uri(request.UrlAddress!).Host,
            LastUse = null,
            Browser = null,
            Os = null,
            Link = link,
        };
        await _appDbContext.LinkStatistics.AddAsync(linkStatistic);

        Geolocation geolocation = new Geolocation() { LinkStatistic = linkStatistic };
        await _appDbContext.Geolocations.AddAsync(geolocation);

        await _appDbContext.SaveChangesAsync();

        return Result<LinkCreatedResponse>.Success(new LinkCreatedResponse(link.Id, link.UrlShort));
    }
}