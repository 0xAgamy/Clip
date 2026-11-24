using System.ComponentModel.DataAnnotations;
using Clips.Domain.Enums;

namespace Clips.Domain.Models
{
    public class Video
    {

        public int Id {get;set;}
        public Guid PublicId {get;set;} = Guid.NewGuid();
        public string UserId {get;set;}= string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description {get;set;}

        public int? GameId { get; set; }
        public string StorageKey { get; set; } = string.Empty;

         public string? ThumbnailKey { get; set; }

        public string? PreviewKey { get; set; }

        public int DurationSeconds { get; set; }

        public long SizeBytes { get; set; }

        public VideoStatus Status { get; set; } =VideoStatus.Processing;

        public VideoVisibility Visibility { get; set; } = VideoVisibility.Public;

        public long ViewCount { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeletedAt { get; set; }


         // Navigation properties
        public User User { get; set; } = null!;

        public Game? Game { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public ICollection<VideoRendition> Renditions { get; set; } = new List<VideoRendition>();

        public ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();

        public ICollection<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();

    }   
}
