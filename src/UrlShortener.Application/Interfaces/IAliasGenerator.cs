namespace UrlShortener.Application.Interfaces;

public interface IAliasGenerator
{
    string GenerateAlias(string url);
}
