using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Responses;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, CommandResult<Guid>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IAliasCteater _shortNameGenerator;

    public CreateLinkCommandHandler(IAppDbContext appDbContext, IAliasCteater shortNameGenerator)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _shortNameGenerator = shortNameGenerator ?? throw new ArgumentNullException(nameof(shortNameGenerator));
    }

    public async Task<CommandResult<Guid>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        CommandResult<Guid> result = new();

        Link? existinglink = await _appDbContext.Links.AsNoTracking()
                                            .Include(l => l.LinkInfo)
                                            .FirstOrDefaultAsync(l => l.UrlAddress == request.CreteLinkDto.UrlAddress);
        if (existinglink != null)
        {
            result.ReturnedObject = existinglink.Id;
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.CreteLinkDto.Alias) == false //if alias != null
            && await AliasIsBusy(request.CreteLinkDto.Alias))
        {
            result.IsSuccess = false;
            result.ErrorMessage = "Alias is taken";
            return result;
        }

        Link link = new Link()
        {
            UrlAddress = request.CreteLinkDto.UrlAddress,
            Alias = request.CreteLinkDto.Alias == null ?
                        _shortNameGenerator.CreateAlias(request.CreteLinkDto.UrlAddress)
                        : request.CreteLinkDto.Alias
        };
        await _appDbContext.Links.AddAsync(link);

        LinkInfo linkInfo = new LinkInfo()
        {
            DomainName = new Uri(request.CreteLinkDto.UrlAddress).Host,
            LastUse = DateTime.UtcNow,
            Link = link,
        };
        await _appDbContext.LinkInfos.AddAsync(linkInfo);

        await _appDbContext.SaveChangesAsync();

        result.ReturnedObject = link.Id;
        result.IsSuccess = true;
        return result;
    }


    private async Task<bool> AliasIsBusy(string alias)
    {
        return await _appDbContext.Links.AnyAsync(l => l.Alias == alias);
    }
}