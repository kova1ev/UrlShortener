using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Domain.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Entity;

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
        if (request.Alias != null && await _linkService.AliasIsBusy(request.Alias, cancellationToken))
        {
            return Result<LinkCreatedResponse>.Failure(new string[] { LinkValidationErrorMessage.AliasTaken });
        }

        if (request.Alias == null)
        {
            LinkCreatedResponse? existingLink = await _appDbContext.Links
                .AsNoTracking()
                .Where(link => link.UrlAddress == request.UrlAddress!.TrimEnd('/').ToLower())
                .Select(link => new LinkCreatedResponse(link.Id, link.UrlShort!))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingLink != null)
            {
                return Result<LinkCreatedResponse>.Success(existingLink);
            }
        }

        string alias = request.Alias ?? await _linkService.GenerateAlias(cancellationToken);

        Geolocation geolocation = new Geolocation();
        await _appDbContext.Geolocations.AddAsync(geolocation, cancellationToken);

        LinkStatistic linkStatistic = new LinkStatistic()
        {
            DomainName = new Uri(request.UrlAddress!).Host,
            LastUse = null,
            Browser = null,
            Os = null,
            Geolocation = geolocation
        };
        await _appDbContext.LinkStatistics.AddAsync(linkStatistic, cancellationToken);

        Link link = new Link()
        {
            UrlAddress = request.UrlAddress?.TrimEnd('/').ToLower(),
            Alias = alias,
            UrlShort = _linkService.CreateShortUrl(alias),
            LinkStatistic = linkStatistic
        };
        await _appDbContext.Links.AddAsync(link, cancellationToken);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return Result<LinkCreatedResponse>.Success(new LinkCreatedResponse(link.Id, link.UrlShort));
    }
}