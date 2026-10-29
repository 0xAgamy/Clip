using Clips_api.Data;
using Clips_api.DTOs.Video;
using Clips_api.Models;
using Clips_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;

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
        [HttpGet("recommend/{id}")]
        public async Task<IActionResult> RecommendVideos(int id)
        {
            var Clips = await _videoServices.RecommendAsync(id);
            return Ok(Clips);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoWithID(int id)
        {
            var video = await _videoServices.GetVideoAsync(id);
            if (video == null) return NotFound("The Video not Exist");
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");
            return Ok(video);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserVideos(string userId)
        {
            var UserVideos = await _videoServices.GetUserVideosAsync(userId);
            if (!ModelState.IsValid) return BadRequest("something bad Happen");
            return Ok(UserVideos);
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadVideo([FromForm]VideoDto model)
        {
            var video = await _videoServices.Upload(model);
            return Ok(video);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClip(int id)
        {
            var clip = await _videoServices.DeleteAsync(id);
            if (clip == null) return NotFound("The clip not found");           
            return Ok(clip);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditClip(int id ,EditVideoDto model)
        {
            var video = await _videoServices.EditAsync(id, model);
            if (video == null) return NotFound("The Edited Video Not Found");
            if (!ModelState.IsValid) return BadRequest("Something bad Happen");        
            return Ok(video);
        }
    }
}
