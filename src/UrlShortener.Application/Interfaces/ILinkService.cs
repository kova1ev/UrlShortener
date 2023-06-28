namespace UrlShortener.Application.Interfaces;

public interface ILinkService
{
    Task<bool> AliasIsBusy(string alias,CancellationToken cancellationToken = default);
    Task<string> GenerateAlias(CancellationToken cancellationToken = default);
    string CreateShortUrl(string alias);

    string RemoveWhiteSpacesFromAlisa(string alias);
}
