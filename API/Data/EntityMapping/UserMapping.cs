using System;
using Clips_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clips_api.Data.EntityMapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        
       builder.Property(u=>u.DisplayName).HasMaxLength(50).IsRequired();


        builder.Property(u=>u.Bio).HasMaxLength(500).IsRequired();
        builder.Property(u=>u.AvatarKey).HasMaxLength(512);

    }
}
