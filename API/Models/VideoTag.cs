using System;

namespace Clips_api.Models;

public class VideoTag
{

    public int VideoId { get; set; }

    public int TagId { get; set; }

    public Video Video { get; set; } = null!;

    public Tag Tag { get; set; } = null!;

}
