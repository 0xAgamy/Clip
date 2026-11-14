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
            
            return Ok(allVideo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoWithID(int id)
        {
            var video = await _videoServices.GetVideoAsync(id);
            if (video == null) return NotFound("The Video not Exist");
            
            return Ok(video);
        }
        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserVideos()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserVideos = await _videoServices.GetUserVideosAsync(userId);
            
            return Ok(UserVideos);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadVideo([FromForm]UploadVideoDto model)
        {
            
            var userEmail= User.FindFirstValue(ClaimTypes.Email);
            try{
                
            await _videoServices.UploadAsync(model,userEmail);
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
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clip = await _videoServices.DeleteAsync(id,userId!);
            if (clip == null) return NotFound("The clip not found");           
            return Ok(clip);
        }


        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> EditClip(int id ,EditVideoDto model)
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);

            var video = await _videoServices.EditAsync(id, userId,model);
            if (video == null) return NotFound("The Edited Video Not Found");
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");        
            return Ok(video);
        }
    }
}
