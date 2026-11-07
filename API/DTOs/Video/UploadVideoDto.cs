namespace Clips_api.DTOs.Video
{
    public class UploadVideoDto
    {
        public IFormFile VideoFile { get; set; }
        public IFormFile thumbnail { get; set; }
        public string UserEmail { get; set; }
        public string title { get; set; }
    }
}
