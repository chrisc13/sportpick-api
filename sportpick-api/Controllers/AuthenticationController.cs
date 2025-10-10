using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sportpick_bll;
using sportpick_domain;
using sportpick_api.DTO;


namespace sportpick_api.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
       // private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;


        public AuthenticationController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.LoginAsync(request.Username, request.Password);
            if (user == null){
                return Unauthorized(new AuthResponse(null, "", "Login failed. Please try again"));
            }
            var token = _tokenService.GenerateToken(user.Username, user.Id ?? "");
            var userResponse = new UserResponse(user.Username, user.Id, user.ProfileImageUrl);
            return Ok(new AuthResponse(userResponse, token, "Success login"));
        }
        
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] LoginRequest request)
        {
            var result = await _authService.RegisterAsync(request.Username, request.Password);

            // result.AppUser can be null if registration failed
            if (result.AppUser == null)
                return BadRequest(result.Message);

            var token = _tokenService.GenerateToken(result.AppUser.Username, result.AppUser.Id ?? "");
            var userResponse = new UserResponse(result.AppUser.Username, result.AppUser.Id, result.AppUser.ProfileImageUrl);

            return Ok(new AuthResponse(userResponse, token, result.Message));
        }

        [HttpPost("ProfileImages")]
        public async Task<IActionResult> GetProfileImages([FromBody] string[] usernames)
        {
            if (usernames == null || usernames.Length == 0)
                return BadRequest("Usernames are required.");

            var images = await _authService.GetProfileImagesForUsernamesAsync(usernames);

            return Ok(images);
        }
       [HttpGet("ProfileImages/{username}")]
        public async Task<IActionResult> GetProfileImage([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");

            var image = await _authService.GetProfileImageForUsernameAsync(username);

            return Ok(image);
        }

    }
}
