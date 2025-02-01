using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WorkoutApi.Models;
using WorkoutApi.Services;

namespace WorkoutApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaxesController : ControllerBase
    {

        private readonly IMaxesService _maxesService;

        public MaxesController(IMaxesService maxesService)
        {
            _maxesService = maxesService;
        }

        [Authorize]
        [HttpGet("GetMaxes")]
        public IActionResult GetProgress()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Missing or invalid Authorization header.");
                }
                var token = authHeader.Substring("Bearer ".Length).Trim();

                MaxModel progress = _maxesService.GetMaxes(token);
                return Ok(progress);
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        [Authorize]
        [HttpPost("UpdateMaxes")]
        public IActionResult UpdateMaxes([FromBody] MaxModel maxModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Missing or invalid Authorization header.");
                }
                var token = authHeader.Substring("Bearer ".Length).Trim();

                _maxesService.UpdateMaxes(token, maxModel);
                return Ok("Maxes Successfully Updated");
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}
