using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

internal class AliasCreater : IAliasCteater
{
    //TODO Implement creater!!
    public string CreateAlias(string url)
    {
        // check generated shortName in DB
        // generate again ? 
        int hash = url.GetHashCode();
        return hash.ToString().TrimStart('-');
    }
}
