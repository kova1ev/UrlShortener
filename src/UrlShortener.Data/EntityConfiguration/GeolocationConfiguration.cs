using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Entity;
using UrlShortener.Entity.Constraints;

namespace UrlShortener.Data.EntityConfiguration;

internal class GeolocationConfiguration : IEntityTypeConfiguration<Geolocation>
{
    public void Configure(EntityTypeBuilder<Geolocation> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Property(l => l.Country).HasMaxLength(GeolocationConstraints.MaxCountryLength);
        builder.Property(l => l.Region).HasMaxLength(GeolocationConstraints.MaxRegionLength);
        builder.Property(l => l.City).HasMaxLength(GeolocationConstraints.MaxCityLength);
    }
}
