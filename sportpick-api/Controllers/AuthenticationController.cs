using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sportpick_bll;
using sportpick_domain;

namespace sportpick_api.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
       // private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] User request)
        {
            var user = _authService.Login(request);
            if (user == null){
                return Unauthorized("Login failed. Please try again");
            }

            return Ok(user);
        }
        
        [HttpPost("Register")]
         public IActionResult Register([FromBody] User request)
        {
            var result = _authService.Register(request);
            if (!result){
                return BadRequest();
            }

            return Ok(true);
        }

    }
}