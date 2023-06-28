using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Entity;

namespace UrlShortener.Data.EntityConfiguration;

internal class GeolocationConfiguration : IEntityTypeConfiguration<Geolocation>
{
    public void Configure(EntityTypeBuilder<Geolocation> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Property(l => l.Country).HasMaxLength(50);
        builder.Property(l => l.Region).HasMaxLength(50);
        builder.Property(l => l.City).HasMaxLength(50);
    }
}
