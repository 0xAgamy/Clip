using Clips_api.DTOs.Video;
using Clips_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clips_api.Services
{
    public interface IvideoServices
    {
        // Get Methods
        public  Task<IEnumerable<Video>> GetAllAsync();
        public Task<IEnumerable<Video>> RecommendAsync(int id);

        public Task<IEnumerable<Video>> GetUserVideosAsync(string userId);

        public Task<Video> GetVideoAsync(int id);

        // Post Methods
        public Task<Video> Upload(VideoDto model);

        // Edit

        public Task<Video> EditAsync(int id, EditVideoDto model);

        // Delete
        public Task<Video> DeleteAsync(int id);
    }
}
