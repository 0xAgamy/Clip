using System.ComponentModel.DataAnnotations;
using Clips_api.Validator;

namespace Clips_api.DTOs.Video
{
    public class UploadVideoDto
    {
        [Required]
        [AllowedExtensions(".mp4", ".webm", ".mov")]
        [MaxFileSize(500 * 1024 * 1024)]
        public IFormFile VideoFile { get; set; }

        [Required]
        [AllowedExtensions(".jpg", ".png", ".jpeg")]
        [MaxFileSize(5 * 1024 * 1024)]


        public IFormFile Thumbnail { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
    }
}
