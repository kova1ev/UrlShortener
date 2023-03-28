using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Models.Links;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Result<LinkResponse>>
{
    private readonly AppDbContext _appDbContext;
    private readonly ILinkService _linkService;

    public CreateLinkCommandHandler(AppDbContext appDbContext, ILinkService aliasService)
    {
        this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _linkService = aliasService ?? throw new ArgumentNullException(nameof(aliasService));
    }

    public async Task<Result<LinkResponse>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        if (request.Alias != null && await _linkService.AliasIsBusy(request.Alias))
        {
            return Result<LinkResponse>.Failure(new string[] { LinkValidationErrorMessage.ALIAS_TAKEN });
        }

        if (request.Alias == null)
        {
            LinkResponse? existingLink = await _appDbContext.Links
                .AsNoTracking()
                .Where(l => l.UrlAddress == request.UrlAddress)
                .Select(l => new LinkResponse(l.Id, l.UrlShort))
                .FirstOrDefaultAsync();

            if (existingLink != null)
            {
                return Result<LinkResponse>.Success(existingLink);
            }
        }

        string alias = request.Alias ?? await _linkService.GenerateAlias();

        Link link = new Link()
        {
            UrlAddress = request.UrlAddress!,
            Alias = alias,
            UrlShort = _linkService.CreateShortUrl(alias)
        };
        _appDbContext.Entry(link).State = EntityState.Added;

        LinkStatistic linkStatistic = new LinkStatistic()
        {
            DomainName = new Uri(request.UrlAddress!).Host,
            LastUse = DateTime.UtcNow,
            Link = link,
        };
        _appDbContext.Entry(linkStatistic).State = EntityState.Added;

        await _appDbContext.SaveChangesAsync();

        return Result<LinkResponse>.Success(new LinkResponse(link.Id, link.UrlShort));
    }
}