using Clips.Application.DTOs.Video;

namespace Clips.Application.Interfaces;

public interface IVideoService
{
    Task<IEnumerable<VideoDto>> GetAllAsync();
    Task<VideoDto?> GetVideoAsync(int id);
    Task<IEnumerable<VideoDto>> GetUserVideosAsync(string userId);
    Task<VideoDto> UploadAsync(UploadVideoDto model, string userEmail);
    Task<VideoDto?> DeleteAsync(int id, string userId);
    Task<VideoDto?> EditAsync(int id, string userId, EditVideoDto model);
}
