namespace UrlShortener.Application.Interfaces;

public interface ILinkService
{
    Task<bool> AliasIsBusy(string alias);
    Task<string> GenerateAlias(string urlAddress);
    string CreateShortUrl(string alias);
}
