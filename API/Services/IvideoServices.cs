using Clips_api.DTOs.Video;
using Clips_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clips_api.Services
{
    public interface IvideoServices
    {
         Task<IEnumerable<VideoDto>> GetAllAsync();
        Task<VideoDto?> GetVideoAsync(int id);
        Task<IEnumerable<VideoDto>> GetUserVideosAsync(string userId);
        Task<VideoDto> UploadAsync(UploadVideoDto model, string userEmail);
        Task<VideoDto?> DeleteAsync(int id, string userId);
        Task<VideoDto?> EditAsync(int id, string userId, EditVideoDto model);
    }
}
