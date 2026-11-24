using System;

namespace Clips_api.Models;

public class Like
{
    public int Id { get; set; }

    public int VideoId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Video Video { get; set; } = null!;

    public User User { get; set; } = null!;


}
