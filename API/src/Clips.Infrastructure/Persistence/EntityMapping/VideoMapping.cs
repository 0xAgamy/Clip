
using Clips.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clips.Infrastructure.Persistence.EntityMapping;

public class VideoMapping : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
       builder.HasKey(v=>v.Id);

       builder.HasIndex(v=>v.PublicId).IsUnique();
       builder.HasIndex(v=>v.StorageKey).IsUnique();

        builder.Property(v => v.Title).HasMaxLength(200).IsRequired();

        builder.Property(v=>v.Description).HasMaxLength(5000);

        builder.Property(v=>v.StorageKey).HasMaxLength(512).IsRequired();

        builder.Property(v=>v.ThumbnailKey).HasMaxLength(512).IsRequired();

        builder.Property(v => v.PreviewKey).HasMaxLength(512);

        builder.HasOne(v => v.User)
            .WithMany(u => u.Videos)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

         builder.HasOne(v => v.Game)
            .WithMany(g => g.Videos)
            .HasForeignKey(v => v.GameId)
            .OnDelete(DeleteBehavior.SetNull);


    }
}
