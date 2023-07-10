namespace UrlShortener.Entity.Constraints;

public static class LinkConstraints
{
    public const int MaxUrlAddressLength = 2000;
    public const int MaxAliasLength = 50;
    public const int MaxUrlShortLength = 75;
}

public static class LinkStatisticConstraints
{
    public const int MaxDomainNameLength = 50;
    public const int MaxBrowserLength = 50;
    public const int MaxOsLength = 50;
}

public static class GeolocationConstraints
{
    public const int MaxCountryLength = 50;
    public const int MaxRegionLength = 50;
    public const int MaxCityLength = 50;
}