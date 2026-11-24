using Clips.Application.DTOs.Video;
using Clips.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Clips.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
        
            _videoService = videoService;
        }

        [HttpGet]

        public async Task<IActionResult> AllVideos()
        {
            var allVideo = await _videoService.GetAllAsync();
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");
            
            return Ok(allVideo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoWithID(int id)
        {
            var video = await _videoService.GetVideoAsync(id);
            if (video == null) return NotFound("The Video not Exist");
            
            return Ok(video);
        }
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserVideos()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserVideos = await _videoService.GetUserVideosAsync(userId!);
            
            return Ok(UserVideos);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadVideo([FromForm]UploadVideoDto model)
        {
            
            var userEmail= User.FindFirstValue(ClaimTypes.Email);
            var video= await _videoService.UploadAsync(model,userEmail!);
            return Ok(video);
            
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteClip(int id)
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clip = await _videoService.DeleteAsync(id,userId!);
            if (clip == null) return NotFound("The clip not found");           
            return Ok(clip);
        }


        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> EditClip(int id ,EditVideoDto model)
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);

            var video = await _videoService.EditAsync(id, userId!, model);
            if (video == null) return NotFound("The Edited Video Not Found");
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");        
            return Ok(video);
        }
    }
}
