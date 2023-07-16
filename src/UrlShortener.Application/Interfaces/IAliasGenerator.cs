namespace UrlShortener.Application.Interfaces;

public interface IAliasGenerator
{
    /// <summary>
    /// Generate a random Alias(string) of Latin uppercase and lowercase letters that is within the specified range.
    /// </summary>
    /// <param name="minLength">Set minimum alias length. Must be positive number.</param>
    /// <param name="maxLength">Set maximum alias length. Must be positive number and greater than <paramref name="minLength"/>.</param>
    /// <returns>Returns a random Alias.</returns>
    /// <exception cref="ArgumentException"><paramref name="minLength"/> must be greater than 0.</exception>
    /// <exception cref="ArgumentException"><paramref name="maxLength"/> must be greater than <paramref name="minLength"/>.</exception>
    string GenerateAlias(int minLength = 4, int maxLength = 10);
}
