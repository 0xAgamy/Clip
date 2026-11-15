
using Clips_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clips_api.Data.EntityMapping;

public class VideoMapping : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasIndex(v=>v.UserId);
        builder.HasIndex(v=>v.Created);

        builder.Property(v=>v.Title).HasMaxLength(200).IsRequired();
        builder.Property(v=>v.VideoUrl).HasMaxLength(512).IsRequired();
        builder.Property(v=>v.ThumbnailUrl).HasMaxLength(512).IsRequired();
        builder.Property(v=>v.UserId).HasMaxLength(64).IsRequired();


    }
}
