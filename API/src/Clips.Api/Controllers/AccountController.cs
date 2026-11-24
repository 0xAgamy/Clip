using Clips.Application.DTOs.Account;
using Clips.Application.Interfaces;
using Clips.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clips.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IJwtService jwtService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto modal)
        {
            var user = await _userManager.FindByEmailAsync(modal.Email);
            if (user== null) return Unauthorized("Invalid Email Or Password");
            if (user.EmailConfirmed == false) return Unauthorized("Please,Check Your Email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, modal.Password,false);

            if (!result.Succeeded)
                return Unauthorized("invalid email or password");
            
            return  CreateUser(user);

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto modal)
        {
            if (await CheckEmailExixt(modal.Email)) return BadRequest("Email is Already Exit");
            var addingUser = new User
            {
                UserName = modal.UserName.ToLower(),
                Email = modal.Email.ToLower(),
                EmailConfirmed = true,
                PhoneNumber = modal.PhoneNumber,
                
            };
            var result =await _userManager.CreateAsync(addingUser,modal.Password);
            if (!result.Succeeded) return BadRequest("Something wrong happen");
            return Ok();

        }
        [Authorize]
        [HttpGet("Refresh-User-Token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email) ?? string.Empty);
             if (user == null) return Unauthorized("User no longer exists");
            return CreateUser(user);
        }
        private  UserDto CreateUser(User user)
        {
            return new UserDto
            {
                id= user.Id,
                Name= user.UserName ?? string.Empty,
                JWT = _jwtService.CreateJwt(user),
                Email= user.Email ?? string.Empty,
            };

            
        }
        private async Task<bool> CheckEmailExixt(string email)
        {
            return await _userManager.Users.AnyAsync(e=>e.Email== email);
        }
    }
}
