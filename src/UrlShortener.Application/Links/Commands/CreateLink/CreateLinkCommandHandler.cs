﻿using MediatR;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Application.Links.Commands.CreateLink;

public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, Guid>
{
    private readonly IAppDbContext _appDbContext;

    public CreateLinkCommandHandler(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
    }

    static int i = 0;
    public async Task<Guid> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        string ShortName;
        if (string.IsNullOrEmpty(request.CreteLinkDto.Alias))
        {
            // TODO  создать короткое имя short name
            ShortName = $"Test {i++}";
        }
        else
        {
            ShortName = request.CreteLinkDto.Alias;
        }

        Link link = new Link(request.CreteLinkDto.UrlAddress, ShortName);

        //LinkInfo linkInfo = new LinkInfo(DateTime.UtcNow, request.CreteLinkDto.UrlAddress, link);


        await _appDbContext.Links.AddAsync(link);
        //await _appDbContext.LinkInfos.AddAsync(linkInfo);

        await _appDbContext.SaveChangesAsync();
        return link.Id;
    }
}
