using System;

namespace Clips_api.Models;

public class PlaylistItem
{
    public long PlaylistId { get; set; }

    public int VideoId { get; set; }

    public int Position { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public Playlist Playlist { get; set; } = null!;

    public Video Video { get; set; } = null!;
}
