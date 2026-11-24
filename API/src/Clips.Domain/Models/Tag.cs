using System;

namespace Clips.Domain.Models;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
