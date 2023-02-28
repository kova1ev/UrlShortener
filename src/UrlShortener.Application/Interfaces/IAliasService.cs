namespace UrlShortener.Application.Interfaces;

public interface IAliasService
{
    Task<bool> AliasIsBusy(string alias);
    Task<string> GenerateAlias(string urlAddress);
}
