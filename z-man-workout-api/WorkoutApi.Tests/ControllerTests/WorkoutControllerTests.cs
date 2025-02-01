using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutApi.Controllers;
using WorkoutApi.Models;
using WorkoutApi.Services;

namespace WorkoutApi.Tests.ControllerTests
{
    public class WorkoutControllerTests
    {
        private readonly WorkoutController _controller;
        private readonly Mock<IWorkoutService> _mockService;

        public WorkoutControllerTests()
        {
            _mockService = new Mock<IWorkoutService>();
            _controller = new WorkoutController(_mockService.Object);
        }

        #region CreateWorkout

        [Fact]
        public void CreateWorkout_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.CreateWorkout(new WorkoutModel { Name="WorkoutName" });

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void CreateWorkout_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.CreateWorkout(new WorkoutModel { Name = "WorkoutName" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateWorkout_ValidRequest_ReturnsOk()
        {
            // Arrange
            var token = "valid-token";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {token}";
            var workoutModel = new WorkoutModel { Name = "Test Workout" };

            // Act
            var result = _controller.CreateWorkout(workoutModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Workout Successfully Created", okResult.Value);
        }

        #endregion

        #region DeleteWorkout

        [Fact]
        public void DeleteWorkout_EmptyWorkoutKey_ReturnsBadRequest()
        {
            // Act
            var result = _controller.DeleteWorkout(Guid.Empty);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("WorkoutKey is Required.", badRequestResult.Value);
        }

        [Fact]
        public void DeleteWorkout_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.DeleteWorkout(Guid.NewGuid());

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        #endregion

        #region GetWorkout

        [Fact]
        public void GetWorkout_ValidAuthorization_ReturnsWorkout()
        {
            // Arrange
            var token = "valid-token";
            var workoutKey = Guid.NewGuid();
            var expectedWorkout = new WorkoutModel { Name = "Test Workout" };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {token}";

            _mockService.Setup(service => service.GetWorkout(token, workoutKey)).Returns(expectedWorkout);

            // Act
            var result = _controller.GetWorkout(workoutKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedWorkout, okResult.Value);
        }

        [Fact]
        public void GetWorkout_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.GetWorkout(Guid.NewGuid());

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        #endregion

        #region GetAllWorkouts

        [Fact]
        public void GetAllWorkouts_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.GetAllWorkouts();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void GetAllWorkouts_ValidAuthorization_ReturnsWorkoutCollection()
        {
            // Arrange
            var token = "valid-token";
            var expectedCollection = new WorkoutCollection();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {token}";

            _mockService.Setup(service => service.GetAllWorkouts(token)).Returns(expectedCollection);

            // Act
            var result = _controller.GetAllWorkouts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCollection, okResult.Value);
        }

        #endregion
    }
}
