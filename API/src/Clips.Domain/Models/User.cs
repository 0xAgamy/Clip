using Microsoft.AspNetCore.Identity;

namespace Clips.Domain.Models
{
    public class User:IdentityUser
    {
        public string DisplayName { get; set; }= string.Empty;
        public string? Bio {get; set;}

        public string? AvatarKey {get; set;}

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;

        public bool IsBanned {get;set;}=false;


        // Navigation properties
        public ICollection<Video> Videos { get; set; } = new List<Video>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();

        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

            
        
    }
}
