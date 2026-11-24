using System;

namespace Clips.Domain.Models;

public class Follow
{
    public string FollowerId { get; set; } = string.Empty;

    public string FollowingId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Follower { get; set; } = null!;

    public User Following { get; set; } = null!;
}
