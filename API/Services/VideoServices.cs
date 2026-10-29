using Clips_api.Data;
using Clips_api.DTOs.Video;
using Clips_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Clips_api.Services
{
    public class VideoServices : IvideoServices
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;
        public VideoServices(ApplicationDbContext context)
        {
            _context = context;
        }

        // All Get Methods
        public async Task<IEnumerable<Video>> GetAllAsync()
        {
            return await _context.Videos.Include(a=>a.user).ToListAsync();
        }

        public async Task<Video> GetVideoAsync(int id)
        {
             var video = await _context.Videos.Include(a=>a.user).FirstOrDefaultAsync(x => x.Id == id);
           return video is not null ? video :null;
        }

        public async Task<IEnumerable<Video>> RecommendAsync(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            var Clips = await _context.Videos.Where(a => a.created > video.created).Take(3).Include(a => a.user).ToListAsync();
            if (Clips.IsNullOrEmpty())
            {
                var allClips = await _context.Videos.OrderByDescending(a => a.Id).Where(a => a.Id != id).Take(3).Skip(0).Include(a => a.user).ToListAsync();
                return allClips;
            }
            return Clips;
        }


        public async Task<IEnumerable<Video>> GetUserVideosAsync(string userId)
        {
            return await _context.Videos.Include(u=>u.user).Where(u => u.UserId == userId).ToListAsync();
        }


        //  Post Methods

        public async Task<Video> Upload(VideoDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserEmail);

            var folderName = Path.Combine("Resources", "Uploads");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);


            var VideoName = ContentDispositionHeaderValue.Parse(model.VideoFile.ContentDisposition).FileName.Trim('"');
            var VdieofullPath = Path.Combine(pathToSave, VideoName);
            var videodbPath = Path.Combine(folderName, VideoName);

            var ImageName = ContentDispositionHeaderValue.Parse(model.thumbnail.ContentDisposition).FileName.Trim('"');
            var ImagefullPath = Path.Combine(pathToSave, ImageName);
            var ImageDbPath = Path.Combine(folderName, ImageName);



            Video video = new()
            {
                Title = model.title,
                videoUrl = videodbPath,
                thumbnailUrl = ImageDbPath,
                thumbnail = model.thumbnail.FileName,
                UserId = user.Id,
            };
            using FileStream filestream = new (VdieofullPath, FileMode.Create);
            using FileStream imagestream = new (ImagefullPath, FileMode.Create);


            model.VideoFile.CopyTo(filestream);
            model.thumbnail.CopyTo(imagestream);
            filestream.Close();
            imagestream.Close();

            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();

            return video;
        }

        // Delete
        public async Task<Video> DeleteAsync(int id)
        {
            var video = await GetVideoAsync(id);
            if (video == null) return video;


            System.IO.File.Delete(video.videoUrl);
            System.IO.File.Delete(video.thumbnailUrl);


            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return video;
        }

        // Edit 
        public async Task<Video> EditAsync(int id, EditVideoDto model)
        {
            var video = await GetVideoAsync(id);
            if (video == null) return video;

            video.Title = model.Title;
            await _context.SaveChangesAsync();
            return video;
        }
    }
}
