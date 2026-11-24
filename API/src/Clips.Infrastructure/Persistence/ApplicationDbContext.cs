using Clips.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clips.Infrastructure.Persistence
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> o):base(o)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Video>().ToTable("Videos");
            builder.Entity<Game>().ToTable("Games");
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Like>().ToTable("Likes");

            builder.Entity<Follow>().ToTable("Follows");
            builder.Entity<Playlist>().ToTable("Playlists");
            builder.Entity<PlaylistItem>().ToTable("PlaylistItems");

            builder.Entity<Tag>().ToTable("Tags");
            builder.Entity<VideoTag>().ToTable("VideoTags");
            builder.Entity<VideoRendition>().ToTable("VideoRenditions");

            builder.Entity<Notification>().ToTable("Notifications");
            builder.Entity<RefreshToken>().ToTable("RefreshTokens");            
        }

    }
}
