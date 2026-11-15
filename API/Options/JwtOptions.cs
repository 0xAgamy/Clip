using System;
using System.ComponentModel.DataAnnotations;

namespace Clips_api.Options;

public class JwtOptions
{
    public const string SectionName = "JWT";

    [Required, MinLength(32)]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Range(1, 100_80)]                                 // 1..10,080 minutes (7 days)
    public int LifetimeMinutes { get; set; } = 60 * 24;

}
