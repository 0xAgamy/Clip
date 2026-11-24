using Clips.Application.DTOs.Video;
using Clips.Application.Interfaces;
using Clips.Domain.Models;
using Clips.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Clips.Infrastructure.Services
{
    public class VideoService : IVideoService
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _storage;
        private readonly ILogger<VideoService> _logger;

        public VideoService(ApplicationDbContext context, UserManager<User> userManager,
                            IFileStorageService storage, ILogger<VideoService> logger)
        {
            _context = context;
            _userManager = userManager;
            _storage = storage;
            _logger = logger;
        }

        private async Task<VideoDto> ToDtoAsync(Video v)
        {
            var videoUrl = await _storage.GetPresignedUrlAsync(v.StorageKey);
            var thumbnailUrl = string.IsNullOrEmpty(v.ThumbnailKey)
                ? string.Empty
                : await _storage.GetPresignedUrlAsync(v.ThumbnailKey);
            return new VideoDto
            {
                Id = v.Id,
                Title = v.Title,
                videoUrl = videoUrl,
                thumbnailUrl = thumbnailUrl,
                UserName = v.User?.UserName ?? string.Empty,
                created = v.CreatedAt
            };

        }
        private async Task<IEnumerable<VideoDto>> ToDtosAsync(IEnumerable<Video> list)
        {
            var items = list.ToList();

            var tasks = items.Select(ToDtoAsync);
            return await Task.WhenAll(tasks);
        }


        // All Get Methods
        public async Task<IEnumerable<VideoDto>> GetAllAsync()
        {
            var videos = await _context.Set<Video>().Include(a => a.User).ToListAsync();
            if (videos is null) return null;
            return await ToDtosAsync(videos);

        }

        public async Task<VideoDto?> GetVideoAsync(int id)
        {
            var video = await _context.Set<Video>().Include(a => a.User).FirstOrDefaultAsync(x => x.Id == id);
            if (video == null) return null;
            return await ToDtoAsync(video);


        }


        public async Task<IEnumerable<VideoDto>> GetUserVideosAsync(string userId)
        {
            var userVideos = await _context.Set<Video>().Include(u => u.User).Where(u => u.UserId == userId).ToListAsync();
            return await ToDtosAsync(userVideos);
        }


        //  Post Methods

        public async Task<VideoDto> UploadAsync(UploadVideoDto model, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail)
                        ?? throw new InvalidOperationException("No User for Email Claim");

            string? videoKey = null, thumbKey = null;
            try
            {
                await using var videoStream = model.VideoFile.OpenReadStream();
                videoKey = await _storage.UploadAsync(videoStream, model.VideoFile.FileName,
                                                    model.VideoFile.ContentType, "videos");

                await using var ts = model.Thumbnail.OpenReadStream();
                thumbKey = await _storage.UploadAsync(ts, model.Thumbnail.FileName,
                                                        model.Thumbnail.ContentType, "thumbnails");


                var video = new Video
                {
                    Title = model.Title,
                    UserId = user.Id,
                    StorageKey = videoKey!,
                    ThumbnailKey = thumbKey,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Set<Video>().Add(video);
                await _context.SaveChangesAsync();
                return await ToDtoAsync(video);

            }
            catch
            {
                if (videoKey != null) await _storage.DeleteAsync(videoKey);
                if (thumbKey != null) await _storage.DeleteAsync(thumbKey);
                throw;
            }
        }

        // Delete
        public async Task<VideoDto?> DeleteAsync(int id, string userId)
        {

            var video = await _context.Set<Video>()
                                     .Include(v => v.User)
                                     .FirstOrDefaultAsync(v => v.Id == id);
            if (video is null || video.UserId != userId) return null;

            _context.Set<Video>().Remove(video);
            await _context.SaveChangesAsync();

            try
            {
                await _storage.DeleteAsync(video.StorageKey);
                if (!string.IsNullOrEmpty(video.ThumbnailKey))
                    await _storage.DeleteAsync(video.ThumbnailKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Orphaned S3 objects: {VideoKey}, {ThumbKey}",
                               video.StorageKey, video.ThumbnailKey);
            }
            return await ToDtoAsync(video);

        }

        // Edit 
        public async Task<VideoDto?> EditAsync(int id, string userId, EditVideoDto model)
        {
            var video = await _context.Set<Video>()
                                     .Include(v => v.User)
                                     .FirstOrDefaultAsync(v => v.Id == id);
            if (video is null || video.UserId != userId) return null;



            video.Title = model.Title;

            await _context.SaveChangesAsync();
            return await ToDtoAsync(video);
        }
    }
}
