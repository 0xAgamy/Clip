using Amazon.S3.Model;
using Clips_api.Data;
using Clips_api.DTOs.Video;
using Clips_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clips_api.Services
{
    public class VideoServices : IvideoServices
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _storage;

        public VideoServices(ApplicationDbContext context, UserManager<User> userManager, IFileStorageService storage)
        {
            _context = context;
            _userManager = userManager;
            _storage=storage;
        }

        private async Task<VideoDto> ToDtoAsync(Video v)
        {
            var videoUrl= await _storage.GetPresignedUrlAsync(v.videoUrl);
            var thumbnailUrl= await _storage.GetPresignedUrlAsync(v.thumbnailUrl);   
            return new VideoDto
            {
                Id= v.Id, Title= v.Title,
                videoUrl= videoUrl, thumbnailUrl= thumbnailUrl,
                UserName= v.user.UserName, created= v.created
            };

        }
        private async Task<IEnumerable<VideoDto>>ToDtosAsync(IEnumerable<Video> list)
        {
            var items= list.ToList();
            var urls= await Task.WhenAll(items.Select(async v => new {v.Id, Video=v}));


            var tasks= items.Select(ToDtoAsync);
            return await Task.WhenAll(tasks);
        }


        // All Get Methods
        public async Task<IEnumerable<VideoDto>> GetAllAsync()
        {
            var videos = await _context.Videos.Include(a=>a.user).ToListAsync();
            if (videos is null) return null;
            return await ToDtosAsync(videos);

        }

        public async Task<VideoDto?> GetVideoAsync(int id)
        {
            var video = await _context.Set<Video>().Include(a => a.user).FirstOrDefaultAsync(x => x.Id == id);
            if (video == null) return null;
            return await ToDtoAsync(video);

    
        }


        public async Task<IEnumerable<VideoDto>> GetUserVideosAsync(string userId)
        {
            var userVideos= await _context.Videos.Include(u=>u.user).Where(u => u.UserId == userId).ToListAsync();
            return await ToDtosAsync(userVideos);
        }


        //  Post Methods

        public async Task<VideoDto> UploadAsync(UploadVideoDto model, string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            using var videoStream= model.VideoFile.OpenReadStream();
            using var thumbStream= model.thumbnail.OpenReadStream();
            
            var videoKey= await _storage.UploadAsync(videoStream, model.VideoFile.FileName,
                                                    model.VideoFile.ContentType,"videos");

            var thumbnailKey = await _storage.UploadAsync(thumbStream, model.thumbnail.FileName,
                                                         model.thumbnail.ContentType, "thumbnails");
            
            var video = new Video
            {
                Title        = model.title,
                UserId       = user!.Id,
                videoUrl     = videoKey,      
                thumbnail    = model.thumbnail.FileName,  
                thumbnailUrl = thumbnailKey,  
                created      = DateTime.UtcNow
            };


            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();

            return await ToDtoAsync(video);
        }

        // Delete
        public async Task<VideoDto?> DeleteAsync(int id, string userId)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video is null || video.UserId != userId) return null;
            


            await _storage.DeleteAsync(video.videoUrl);
            await _storage.DeleteAsync(video.thumbnailUrl);
            


            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return await ToDtoAsync(video);

        }

        // Edit 
        public async Task<VideoDto?> EditAsync(int id, string userId ,EditVideoDto model)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video is null || video.UserId != userId) return null;

            

            video.Title = model.Title;

            await _context.SaveChangesAsync();
            return await ToDtoAsync(video);
        }
    }
}
