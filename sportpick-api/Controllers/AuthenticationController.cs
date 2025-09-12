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
        public async Task<IActionResult> Login([FromBody] AppUser request)
        {
            var user = await _authService.LoginAsync(request);
            if (user == null){
                return Unauthorized(new AuthResponse(null, "", "Login failed. Please try again"));
            }
            var token = _tokenService.GenerateToken(user.Username, user.Id ?? "");
            var userResponse = new UserResponse(user.Username);
            return Ok(new AuthResponse(userResponse, token, "Success login"));
        }
        
        [HttpPost("Register")]
         public async Task<ActionResult> Register([FromBody] AppUser request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result == null){
                return BadRequest();
            }

            return Ok(true);
        }

    }
}