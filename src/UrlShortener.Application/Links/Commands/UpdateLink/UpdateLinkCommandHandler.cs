using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.UpdateLink;

public class UpdateLinkCommandHandler : IRequestHandler<UpdateLinkCommand, Result>
{
    private readonly AppDbContext _appDbContext;
    private readonly ILinkService _linkService;

    public UpdateLinkCommandHandler(AppDbContext appDbContext, ILinkService aliasService)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _linkService = aliasService ?? throw new ArgumentNullException(nameof(aliasService));
    }

    public async Task<Result> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        Link? existingLink = await _appDbContext.Links.FirstOrDefaultAsync(l => l.Id == request.Id);
        if (existingLink == null)
        {
            return Result.Failure(new string[] { LinkValidationErrorMessage.LINK_NOT_EXISTING });
        }

        if (request.Alias != null && existingLink.Alias != request.Alias && await _linkService.AliasIsBusy(request.Alias))
        {
            return Result.Failure(new string[] { LinkValidationErrorMessage.ALIAS_TAKEN });
        }

        //todo remove ??
        existingLink.UrlAddress = request.UrlAddress ?? existingLink.UrlAddress;
        existingLink.Alias = request.Alias ?? existingLink.Alias;
        existingLink.UrlShort = _linkService.CreateShortUrl(existingLink.Alias);

        _appDbContext.Entry<Link>(existingLink).State = EntityState.Modified;
        await _appDbContext.SaveChangesAsync();
        return Result.Success();
    }

}
