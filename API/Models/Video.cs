using System.ComponentModel.DataAnnotations;

namespace Clips_api.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public string Title { get;set; }
        public string videoUrl { get; set; }
        public string thumbnail { get; set; }
        public string thumbnailUrl { get; set; }
        public string UserId { get; set; }
        public User? user { get; set; }
        public DateTime? created { get; set; }=DateTime.Now;
    }   
}
