namespace UrlShortener.Application.Interfaces;

public interface IAliasGenerator
{
    string GenerateAlias(int minLength = 4, int maxLength = 10);
}
