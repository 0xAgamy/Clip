using System;

namespace Clips_api.Models;

public class Playlist
{

    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsPublic { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public ICollection<PlaylistItem> Items { get; set; } = new List<PlaylistItem>();
    
}
