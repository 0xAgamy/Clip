namespace Clips.Application.DTOs.Video;

public class VideoDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public string videoUrl { get; set; } = string.Empty;

    public string thumbnailUrl { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public DateTime? created { get; set; }
}
