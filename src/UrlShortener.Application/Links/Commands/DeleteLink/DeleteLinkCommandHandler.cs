using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Common.Result;
using UrlShortener.Data;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.DeleteLink;

public class DeleteLinkCommandHandler : IRequestHandler<DeleteLinkCommand, Result>
{
    private readonly AppDbContext _appDbContext;

    public DeleteLinkCommandHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    public async Task<Result> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {

        Link? link = await _appDbContext.Links.FirstOrDefaultAsync(l => l.Id == request.Id);
        if (link == null)
        {
            return Result.Failure(new[] { $"Link with id '{request.Id}' is not existing" });
        }
        _appDbContext.Entry(link).State = EntityState.Deleted;

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}
