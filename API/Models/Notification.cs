using System;

namespace Clips_api.Models;

public class Notification
{
    public long Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string? Message { get; set; }

    public string? ActorUserId { get; set; }

    public int? VideoId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public User? ActorUser { get; set; }

    public Video? Video { get; set; }
}
