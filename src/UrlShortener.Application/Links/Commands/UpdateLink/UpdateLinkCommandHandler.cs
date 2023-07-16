using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Entity;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommandHandler : IRequestHandler<UpdateLinkCommand, Result>
{
    private readonly IAppDbContext _appDbContext;
    private readonly ILinkService _linkService;

    public UpdateLinkCommandHandler(IAppDbContext appDbContext, ILinkService aliasService)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _linkService = aliasService ?? throw new ArgumentNullException(nameof(aliasService));
    }

    public async Task<Result> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        Link? existingLink = await _appDbContext.Links.Include(l => l.LinkStatistic)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);
        if (existingLink == null)
        {
            return Result.Failure(new string[] { LinkValidationErrorMessage.LinkNotExisting });
        }

        if (request.Alias != null && existingLink.Alias != request.Alias &&
            await _linkService.AliasIsBusy(request.Alias, cancellationToken))
        {
            return Result.Failure(new string[] { LinkValidationErrorMessage.AliasTaken });
        }

        existingLink.UrlAddress = request.UrlAddress?.Trim('/')?.ToLower() ?? existingLink.UrlAddress;
        if (request.UrlAddress != null)
        {
            existingLink.LinkStatistic!.DomainName = new Uri(request.UrlAddress).Host;
        }

        existingLink.Alias = request.Alias ?? existingLink.Alias;
        if (request.Alias != null)
        {
            existingLink.UrlShort = _linkService.CreateShortUrl(request.Alias);
        }

        _appDbContext.Links.Update(existingLink);

        await _appDbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}