using System;
using System.Numerics;
using Clips_api.Enums;

namespace Clips_api.Models;

public class VideoRendition
{
    public int Id { get; set; }

    public int VideoId { get; set; }

    public VideoQuality Quality { get; set; } = VideoQuality.P480;

    public string StorageKey { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public Video Video { get; set; } = null!;
}
