using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Commands.Links;
using UrlShortener.Application.Common;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Result<LinkResponse>>
{
    private const string PROLOCOL = "https://";
    public const string SCHEME = "fgfg";
    private readonly AppDbContext _appDbContext;
    private readonly IAliasService _aliasService;
    private readonly AppOptions _options;

    public CreateLinkCommandHandler(AppDbContext appDbContext, IOptions<AppOptions> options, IAliasService aliasService)
    {
        this._appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _aliasService = aliasService ?? throw new ArgumentNullException(nameof(aliasService));
    }

    public async Task<Result<LinkResponse>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        if (request.Alias != null && await _aliasService.AliasIsBusy(request.Alias))
        {
            return Result<LinkResponse>.Failure(new string[] { "Alias is taken" });
        }

        if (request.Alias == null)
        {
            Link? existingLink = await _appDbContext.Links
                .AsNoTracking()
                .Include(l => l.LinkInfo)
                .FirstOrDefaultAsync(l => l.UrlAddress == request.UrlAddress);
            if (existingLink != null)
            {
                return Result<LinkResponse>.Success(new LinkResponse(existingLink.Id, existingLink.UrlShort));
            }
        }

        string alias = request.Alias == null ? await _aliasService.GenerateAlias(request.UrlAddress) : request.Alias;

        Link link = new Link()
        {
            UrlAddress = request.UrlAddress,
            Alias = alias,
            //TODO  куда-то вынести
            UrlShort = string.Concat(PROLOCOL, _options.HostName, '/', alias)

        };
        _appDbContext.Entry(link).State = EntityState.Added;

        LinkInfo linkInfo = new LinkInfo()
        {
            DomainName = new Uri(request.UrlAddress).Host,
            LastUse = DateTime.UtcNow,
            Link = link,
        };
        _appDbContext.Entry(linkInfo).State = EntityState.Added;

        await _appDbContext.SaveChangesAsync();

        return Result<LinkResponse>.Success(new LinkResponse(link.Id, link.UrlShort));
    }
}