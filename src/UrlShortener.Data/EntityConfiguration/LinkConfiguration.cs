using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data.EntityConfiguration;

internal class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("links");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Property(l => l.UrlAddress).IsRequired();
        builder.Property(l => l.Alias).HasMaxLength(30).IsRequired();
        builder.HasIndex(l => l.Alias).IsUnique();
        builder.Property(l => l.UrlShort).IsRequired();
    }
}
