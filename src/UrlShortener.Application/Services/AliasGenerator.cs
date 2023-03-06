using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

internal class AliasGenerator : IAliasGenerator
{
    public string GenerateAlias(string url)
    {
        //todo Create Normal generate
        int hash = url.GetHashCode();
        return hash.ToString().TrimStart('-');
    }
}
