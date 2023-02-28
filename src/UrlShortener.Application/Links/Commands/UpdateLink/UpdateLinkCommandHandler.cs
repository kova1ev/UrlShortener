using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Common;
using UrlShortener.Application.Interfaces;
using UrlShortener.Common.Result;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommandHandler : IRequestHandler<UpdateLinkCommand, Result>
{
    //todo
    private const string PROLOCOL = "https://";

    private readonly AppDbContext _appDbContext;
    private readonly IAliasService _aliasService;
    private readonly AppOptions _options;

    public UpdateLinkCommandHandler(AppDbContext appDbContext, IAliasService aliasService, IOptions<AppOptions> options)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _aliasService = aliasService ?? throw new ArgumentNullException(nameof(aliasService));
        _options = options.Value ?? throw new ArgumentException(nameof(options));
    }

    public async Task<Result> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        Link? link = await _appDbContext.Links.FirstOrDefaultAsync(l => l.Id == request.Id);
        if (link == null)
        {
            return Result.Failure(new string[] { $"Link not found id {request.Id}" });
        }

        /*if alias != null*/
        // todo move in validator ?!!!!!  
        if (string.IsNullOrWhiteSpace(request.Alias) == false && link.Alias != request.Alias
            && await _aliasService.AliasIsBusy(request.Alias))
        {
            return Result.Failure(new string[] { "Alias is taken" });
        }

        link.Alias = request.Alias ?? link.Alias;
        link.UrlAddress = request.UrlAddress ?? link.UrlAddress;
        //todo
        link.UrlShort = string.Concat(PROLOCOL, _options.HostName, '/', link.Alias);

        _appDbContext.Entry<Link>(link).State = EntityState.Modified;
        await _appDbContext.SaveChangesAsync();
        return Result.Success();
    }

}
