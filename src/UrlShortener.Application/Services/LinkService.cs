﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Common;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

public class LinkService : ILinkService
{
    private const string RedirectRoute = "/r/";
    private readonly IAppDbContext _appDbContext;
    private readonly IAliasGenerator _aliasGenerator;
    private readonly AppOptions _appOptions;

    public LinkService(IAppDbContext appDbContext, IAliasGenerator aliasGenerator, IOptions<AppOptions> options)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _aliasGenerator = aliasGenerator ?? throw new ArgumentNullException(nameof(aliasGenerator));
        _appOptions = options.Value ?? throw new ArgumentNullException(nameof(options));
    }


    public async Task<bool> AliasIsBusy(string alias)
    {
        return await _appDbContext.Links.AnyAsync(l => l.Alias == alias);
    }

    public async Task<string> GenerateAlias()
    {
        string alias;
        do
        {
            alias = _aliasGenerator.GenerateAlias();

        } while (await AliasIsBusy(alias));

        return alias;
    }

    public string CreateShortUrl(string alias)
    {
        return string.Concat(_appOptions.AppUrl, RedirectRoute, alias);
    }
}
