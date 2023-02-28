using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

internal class AliasCreater : IAliasCteater
{

    public string CreateAlias(string url)
    {
        //todo Create Normal generate
        int hash = url.GetHashCode();
        return hash.ToString().TrimStart('-');
    }
}
