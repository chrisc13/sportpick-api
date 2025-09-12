using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sportpick_bll;
using Microsoft.AspNetCore.Authorization;
using sportpick_domain;
using System.Security.Claims;

namespace sportpick_api.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly IProfileService _profileService;


        public ProfileController(ILogger<ProfileController> logger, IProfileService profileService)
        {
            _logger = logger;
            _profileService = profileService;
        }
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            // Get user ID from claims (JWT or Identity)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            if (userId == ""){
                return Unauthorized();
            }

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }
        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertProfile([FromBody] Profile profile)
        {
            if (profile == null)
                return BadRequest("Profile is required.");

            try
            {
                var success = await _profileService.UpsertProfileAsync(profile);
                if (success)
                    return Ok(true);
                else
                    return StatusCode(500, "Failed to upsert profile.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");

            var profile = await _profileService.GetProfileByUsernameAsync(username);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

    }
}