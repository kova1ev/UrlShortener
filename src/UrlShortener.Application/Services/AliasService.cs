using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Data;

namespace UrlShortener.Application.Services;

public class AliasService : IAliasService
{
    private readonly AppDbContext _appDbContext;
    private readonly IAliasCteater _aliasCteater;

    public AliasService(AppDbContext appDbContext, IAliasCteater aliasCteater)
    {
        _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        _aliasCteater = aliasCteater ?? throw new ArgumentNullException(nameof(aliasCteater));
    }


    public async Task<bool> AliasIsBusy(string alias)
    {
        return await _appDbContext.Links.AnyAsync(l => l.Alias == alias);
    }

    public async Task<string> GenerateAlias(string urlAddress)
    {
        string alias = string.Empty;
        do
        {
            alias = _aliasCteater.CreateAlias(urlAddress);

        } while (await AliasIsBusy(alias));

        return alias;
    }
}
