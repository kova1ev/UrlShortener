using System.Text;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

/// <inheritdoc cref="IAliasGenerator"/>
internal class AliasGenerator : IAliasGenerator
{
    private const string Source = "abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLNMOPQRSTUVWXYZ";

    public string GenerateAlias(int minLength = 4, int maxLength = 10)
    {
        if (minLength < 1)
            throw new ArgumentException($"{nameof(minLength)} must be greater than 0.");
        if (maxLength < minLength)
            throw new ArgumentException($"{nameof(maxLength)} must be greater than {nameof(minLength)}.");

        Random rnd = new Random();
        int length = rnd.Next(minLength, maxLength + 1);
        StringBuilder sb = new(length);
        for (int i = 0; i < length; i++)
        {
            int index = rnd.Next(0, Source.Length);
            sb.Append(Source[index]);
        }

        return sb.ToString();
    }
}