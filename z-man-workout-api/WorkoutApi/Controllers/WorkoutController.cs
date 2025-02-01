using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WorkoutApi.Services;
using WorkoutApi.Models;

namespace WorkoutApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [Authorize]
        [HttpPost("CreateWorkout")]
        public IActionResult CreateWorkout([FromBody] WorkoutModel workoutModel)
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

                _workoutService.CreateWorkout(token, workoutModel);
                return Ok("Workout Successfully Created");
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        [Authorize]
        [HttpPost("DeleteWorkout/{workoutKey:guid}")]
        public IActionResult DeleteWorkout(Guid workoutKey)
        {
            if (workoutKey == Guid.Empty)
            {
                return BadRequest("WorkoutKey is Required.");
            }

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Missing or invalid Authorization header.");
                }
                var token = authHeader.Substring("Bearer ".Length).Trim();

                _workoutService.DeleteWorkout(token, workoutKey);
                return Ok("Workout Successfully Deleted");
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        [Authorize]
        [HttpGet("GetWorkout/{workoutKey:guid}")]
        public IActionResult GetWorkout(Guid workoutKey)
        {
            if (workoutKey == Guid.Empty)
            {
                return BadRequest("WorkoutKey is Required.");
            }

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Missing or invalid Authorization header.");
                }
                var token = authHeader.Substring("Bearer ".Length).Trim();

                WorkoutModel workout = _workoutService.GetWorkout(token, workoutKey);
                return Ok(workout);
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        [Authorize]
        [HttpGet("GetAllWorkouts")]
        public IActionResult GetAllWorkouts()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Missing or invalid Authorization header.");
                }
                var token = authHeader.Substring("Bearer ".Length).Trim();

                WorkoutCollection workout = _workoutService.GetAllWorkouts(token);
                return Ok(workout);
            }
            catch (SqlException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }
    }
}
