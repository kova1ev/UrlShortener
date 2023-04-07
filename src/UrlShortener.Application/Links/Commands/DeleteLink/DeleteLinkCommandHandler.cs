﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Constants;
using UrlShortener.Application.Common.Result;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.DeleteLink;

public class DeleteLinkCommandHandler : IRequestHandler<DeleteLinkCommand, Result>
{
    private readonly IAppDbContext _appDbContext;

    public DeleteLinkCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {

        Link? link = await _appDbContext.Links.FirstOrDefaultAsync(l => l.Id == request.Id);
        if (link == null)
        {
            return Result.Failure(new[] { LinkValidationErrorMessage.LINK_NOT_EXISTING });
        }
        _appDbContext.Links.Remove(link);

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}
