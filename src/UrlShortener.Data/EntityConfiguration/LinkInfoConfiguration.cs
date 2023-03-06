using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entity;

namespace UrlShortener.Data.EntityConfiguration;

internal class LinkInfoConfiguration : IEntityTypeConfiguration<LinkInfo>
{
    public void Configure(EntityTypeBuilder<LinkInfo> builder)
    {
        builder.ToTable("linkinfos")
            .HasOne(i => i.Link)
            .WithOne(l => l.LinkInfo)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();
        builder.Property(i => i.DomainName).HasMaxLength(50);
    }
}
