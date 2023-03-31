using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entity;

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
        builder.Property(i => i.DomainName).HasMaxLength(50);
        builder.Property(i => i.Browser).HasMaxLength(50);
        builder.Property(i => i.Os).HasMaxLength(50);
        //builder.Property(i => i.City).HasMaxLength(50);
        //builder.Property(i => i.Region).HasMaxLength(50);
        //builder.Property(i => i.Country).HasMaxLength(50);
    }
}
