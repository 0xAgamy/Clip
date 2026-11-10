using Clips_api.DTOs.Video;
using Clips_api.Services;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Clips_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        private readonly IvideoServices _videoServices;
        public VideoController(IvideoServices videoServices)
        {
        
            _videoServices = videoServices;
        }

        [HttpGet]

        public async Task<IActionResult> AllVideos()
        {
            var allVideo = await _videoServices.GetAllAsync();
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");
            if (allVideo == null) return NotFound("No Videos Found");
            var allVideoList = new List<VideoDto>();

            foreach (var vd in allVideo)
            {
                allVideoList.Add(new VideoDto
                {
                    Id = vd.Id,
                    Title = vd.Title,
                    videoUrl = vd.videoUrl,
                    thumbnailUrl = vd.thumbnailUrl,
                    UserName = vd.user?.UserName,
                    created = vd.created
                });
            }
            return Ok(allVideoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoWithID(int id)
        {
            var video = await _videoServices.GetVideoAsync(id);
            if (video == null) return NotFound("The Video not Exist");
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");

            var videoDto= new VideoDto
            {
                Id = video.Id,
                Title = video.Title,
                videoUrl = video.videoUrl,
                thumbnailUrl = video.thumbnailUrl,
                UserName = video.user?.UserName,
                created = video.created
            };
            return Ok(videoDto);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserVideos(string userId)
        {
            var UserVideos = await _videoServices.GetUserVideosAsync(userId);
            if (!ModelState.IsValid) return BadRequest("something bad Happen");
            return Ok(UserVideos);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadVideo([FromForm]UploadVideoDto model)
        {
            
            var userEmail= User.FindFirstValue(ClaimTypes.Email);
            try{
                
            await _videoServices.Upload(model,userEmail);
            return Ok();

            }
            catch(Exception ex)
            {
                return BadRequest();
            }

            
            
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteClip(int id)
        {
            var clip = await _videoServices.DeleteAsync(id);
            if (clip == null) return NotFound("The clip not found");           
            return Ok(clip);
        }


        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> EditClip(int id ,EditVideoDto model)
        {
            var video = await _videoServices.EditAsync(id, model);
            if (video == null) return NotFound("The Edited Video Not Found");
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");        
            return Ok(video);
        }
    }
}
