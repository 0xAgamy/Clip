using System;
using Clips.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clips.Infrastructure.Persistence.EntityMapping;

public class LikeMapping : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(l=>l.Id);
        
         builder.HasIndex(l => new { l.VideoId, l.UserId })
            .IsUnique();

        builder.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId);

        builder.HasOne(l => l.Video)
            .WithMany(v => v.Likes)
            .HasForeignKey(l => l.VideoId);
    }
}
