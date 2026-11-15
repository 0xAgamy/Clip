using System.ComponentModel.DataAnnotations;

namespace Clips_api.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get;set; } = string.Empty;
        [Required, MaxLength(512)]
        public string VideoUrl { get; set; } = string.Empty;

        [Required, MaxLength(512)]
        public string ThumbnailUrl { get; set; } = string.Empty;
        [Required, MaxLength(64)]
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public DateTime? Created { get; set; }=DateTime.UtcNow;
    }   
}
