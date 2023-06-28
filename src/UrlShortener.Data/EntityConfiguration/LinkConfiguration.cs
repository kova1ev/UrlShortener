using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Entity;
using UrlShortener.Entity.Constraints;

namespace UrlShortener.Data.EntityConfiguration;

internal class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();
        builder.Property(l => l.UrlAddress).HasMaxLength(LinkConstraints.MaxUrlAddressLength).IsRequired();
        builder.Property(l => l.Alias).HasMaxLength(LinkConstraints.MaxAliasLength).IsRequired();
        builder.HasIndex(l => l.Alias).IsUnique();
        builder.Property(l => l.UrlShort).HasMaxLength(LinkConstraints.MaxUrlShortLength).IsRequired();

        //builder.HasIndex(l => l.UrlAddress).IsUnique().HasFilter("LOWER(url_address) = url_address");
        //builder.Property(l => l.UrlAddress).UseCollation("case_insensitivity");

    }
}
