using System.Text;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;


internal class AliasGenerator : IAliasGenerator
{
    private const string SOURCE = "abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLNMOPQRSTUVWXYZ";
    public string GenerateAlias(int minLength = 4, int maxLength = 10)
    {
        if (minLength < 3)
            throw new ArgumentException($"{nameof(minLength)} must be above 3.");
        if (maxLength <= minLength)
            throw new ArgumentException($"{nameof(maxLength)} must be above {nameof(minLength)}.");

        Random rnd = new Random();
        int length = rnd.Next(minLength, maxLength + 1);
        StringBuilder sb = new(length);
        for (int i = 0; i < length; i++)
        {
            int index = rnd.Next(0, SOURCE.Length);
            sb.Append(SOURCE[index]);
        }

        return sb.ToString();
    }
}
