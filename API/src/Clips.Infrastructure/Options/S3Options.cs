
using System.ComponentModel.DataAnnotations;

namespace Clips.Infrastructure.Options;

public class S3Options
{
    public const string SectionName = "S3";

    [Required, Url]
    public string ServiceUrl { get; set; } = string.Empty;

    [Required]
    public string AccessKey { get; set; } = string.Empty;

    [Required]
    public string SecretKey { get; set; } = string.Empty;

    [Required, MinLength(3)]
    public string BucketName { get; set; } = string.Empty;

    public bool ForcePathStyle { get; set; } = true;

    public bool UseHttp { get; set; } = false;

    [Range(1, 1440)]                                   
    public int PresignExpiryMinutes { get; set; } = 120;

    public string? Region { get; set; }

}
