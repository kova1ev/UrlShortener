using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Entity;
using UrlShortener.Entity.Constraints;

namespace UrlShortener.Data.EntityConfiguration;

internal class LinkStatisticConfiguration : IEntityTypeConfiguration<LinkStatistic>
{
    public void Configure(EntityTypeBuilder<LinkStatistic> builder)
    {
        builder.HasOne(i => i.Link)
         .WithOne(l => l.LinkStatistic)
         .OnDelete(DeleteBehavior.Cascade);
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();
        builder.Property(i => i.DomainName).HasMaxLength(LinkStatisticConstraints.MaxDomainNameLength);
        builder.Property(i => i.Browser).HasMaxLength(LinkStatisticConstraints.MaxBrowserLength);
        builder.Property(i => i.Os).HasMaxLength(LinkStatisticConstraints.MaxOsLength);
    }
}
