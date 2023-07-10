using System.Reflection;

namespace UrlShortener.Application;

/// <summary>
///  Represent Application assembly.
/// </summary>
public static class ApplicationAssembly
{
    /// <summary>
    /// Return application assembly.
    /// </summary>
    public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
}