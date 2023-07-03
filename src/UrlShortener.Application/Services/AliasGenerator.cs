using System.Text;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

internal class AliasGenerator : IAliasGenerator
{
    private const string Source = "abcdefghijklnmopqrstuvwxyzABCDEFGHIJKLNMOPQRSTUVWXYZ";

    /// <summary>
    /// Generate a random Alias(string) of Latin uppercase and lowercase letters that is within the specified range.
    /// </summary>
    /// <param name="minLength">Set minimum alias length. Must be positive number.</param>
    /// <param name="maxLength">Set maximum alias length. Must be positive number and greater than <paramref name="minLength"/>.</param>
    /// <returns>Returns a random Alias.</returns>
    /// <exception cref="ArgumentException"><paramref name="minLength"/> must be greater than 0.</exception>
    /// <exception cref="ArgumentException"><paramref name="maxLength"/> must be greater than <paramref name="minLength"/>.</exception>
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