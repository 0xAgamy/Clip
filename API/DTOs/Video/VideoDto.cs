namespace Clips_api.DTOs.Video
{
    public class VideoDto
    {
        public IFormFile VideoFile { get; set; }
        public IFormFile thumbnail { get; set; }
        public string UserEmail { get; set; }
        public string title { get; set; }
    }
}
