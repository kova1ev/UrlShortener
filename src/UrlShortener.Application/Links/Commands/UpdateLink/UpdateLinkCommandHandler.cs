using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Common;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
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
        Link? existingLink = await _appDbContext.Links.FirstOrDefaultAsync(l => l.Id == request.Id);
        if (existingLink == null)
        {
            return Result.Failure(new string[] { $"Link not found id {request.Id}" });
        }

        /*if alias != null*/
        if (request.Alias != null && existingLink.Alias != request.Alias && await _aliasService.AliasIsBusy(request.Alias))
        {
            return Result.Failure(new string[] { "Alias is taken" });
        }

        //todo remove ??
        existingLink.Alias = request.Alias ?? existingLink.Alias;
        existingLink.UrlAddress = request.UrlAddress ?? existingLink.UrlAddress;
        //todo
        existingLink.UrlShort = string.Concat(PROLOCOL, _options.HostName, '/', existingLink.Alias);



        _appDbContext.Entry<Link>(existingLink).State = EntityState.Modified;
        await _appDbContext.SaveChangesAsync();
        return Result.Success();
    }

}
