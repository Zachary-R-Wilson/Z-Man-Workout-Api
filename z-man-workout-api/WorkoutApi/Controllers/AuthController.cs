using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WorkoutApi.Services;
using WorkoutApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace WorkoutApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var accessToken = _authService.AuthenticateUser(credentials);

                if(accessToken == null) 
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(new { AccessToken = accessToken });
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] LoginModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var accessToken = _authService.RegisterUser(credentials);

                if (accessToken == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(new { AccessToken = accessToken });
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}
