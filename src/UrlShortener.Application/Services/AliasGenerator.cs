using System.Text;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

internal class AliasGenerator : IAliasGenerator
{
    public string GenerateAlias(int minLength = 4, int maxLength = 10)
    {
        Random rnd = new Random();
        string source = "abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLNMOPQRSTUVWXYZ";
        int length = rnd.Next(minLength, maxLength + 1);

        StringBuilder sb = new(length);

        for (int i = 0; i < length; i++)
        {
            int index = rnd.Next(0, source.Length);
            sb.Append(source[index]);
        }
        return sb.ToString();

    }
}
