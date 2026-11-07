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

        // All Get Methods
        public async Task<IEnumerable<Video?>> GetAllAsync()
        {
            var videos = await _context.Videos.Include(a=>a.user).ToListAsync();
            if (videos is null) return null;
            foreach (var vd in videos)
            {
                vd.videoUrl = await _storage.GetPresignedUrlAsync(vd.videoUrl);
                vd.thumbnailUrl= await _storage.GetPresignedUrlAsync(vd.thumbnailUrl);

            }
            return videos;
        }

        public async Task<Video> GetVideoAsync(int id)
        {
            var video = await _context.Set<Video>().Include(a => a.user).FirstOrDefaultAsync(x => x.Id == id);
            if (video == null) return null;
            video.videoUrl= await _storage.GetPresignedUrlAsync(video.videoUrl);
            video.thumbnailUrl= await _storage.GetPresignedUrlAsync(video.thumbnailUrl);

            return video;
    
        }

        public async Task<IEnumerable<Video>> RecommendAsync(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null) return Enumerable.Empty<Video>();
            else
            {               
                var Clips = await _context.Videos.Where(a => a.created > video.created).Take(3).Include(a => a.user).ToListAsync();
                if (Clips.Count == 0)
                {
                    var allClips = await _context.Videos.OrderByDescending(a => a.Id).Where(a => a.Id != id).Take(3).Skip(0).Include(a => a.user).ToListAsync();
                    return allClips;
                }
                return Clips;
        }
            }


        public async Task<IEnumerable<Video>> GetUserVideosAsync(string userId)
        {
            return await _context.Videos.Include(u=>u.user).Where(u => u.UserId == userId).ToListAsync();
        }


        //  Post Methods

        public async Task<Video> Upload(UploadVideoDto model)
        {
             var user = await _userManager.FindByEmailAsync(model.UserEmail);

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

            return video;
        }

        // Delete
        public async Task<Video> DeleteAsync(int id)
        {
            var video = await GetVideoAsync(id);
            


            await _storage.DeleteAsync(video.videoUrl);
            await _storage.DeleteAsync(video.thumbnailUrl);
            


            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return video;
        }

        // Edit 
        public async Task<Video> EditAsync(int id, EditVideoDto model)
        {
            var video = await GetVideoAsync(id);
            

            video.Title = model.Title;
            await _context.SaveChangesAsync();
            return video;
        }
    }
}
