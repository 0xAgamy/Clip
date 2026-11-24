using System.ComponentModel.DataAnnotations;
using Clips.Application.Validation;
using Microsoft.AspNetCore.Http;

namespace Clips.Application.DTOs.Video
{
    public class UploadVideoDto
    {
        [Required]
        [AllowedExtensions(".mp4", ".webm", ".mov")]
        [MaxFileSize(500 * 1024 * 1024)]
        public IFormFile VideoFile { get; set; } = null!;

        [Required]
        [AllowedExtensions(".jpg", ".png", ".jpeg")]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile Thumbnail { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
    }
}
