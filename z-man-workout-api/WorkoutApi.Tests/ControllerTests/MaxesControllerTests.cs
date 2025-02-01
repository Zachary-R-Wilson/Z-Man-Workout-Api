using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutApi.Controllers;
using WorkoutApi.Models;
using WorkoutApi.Services;

namespace WorkoutApi.Tests.ControllerTests
{
    public class MaxesControllerTests
    {
        private readonly MaxesController _controller;
        private readonly Mock<IMaxesService> _mockService;

        public MaxesControllerTests()
        {
            _mockService = new Mock<IMaxesService>();
            _controller = new MaxesController(_mockService.Object);
        }

        #region GetProgress

        [Fact]
        public void GetProgress_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.GetProgress();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void GetProgress_ValidAuthorizationHeader_ReturnsOkWithProgress()
        {
            // Arrange
            var expectedToken = "valid-token";
            var expectedProgress = new MaxModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.GetMaxes(expectedToken)).Returns(expectedProgress);

            // Act
            var result = _controller.GetProgress();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedProgress, okResult.Value);
        }

        [Fact]
        public void GetProgress_ServiceThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var expectedToken = "valid-token";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.GetMaxes(expectedToken)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.GetProgress();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion

        #region UpdateMaxes

        [Fact]
        public void UpdateMaxes_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Squat", "Required");

            // Act
            var result = _controller.UpdateMaxes(new MaxModel());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateMaxes_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.UpdateMaxes(new MaxModel());

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void UpdateMaxes_ValidRequest_ReturnsOk()
        {
            // Arrange
            var expectedToken = "valid-token";
            var maxModel = new MaxModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            // Act
            var result = _controller.UpdateMaxes(maxModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Maxes Successfully Updated", okResult.Value);
        }


        [Fact]
        public void UpdateMaxes_ServiceThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var expectedToken = "valid-token";
            var maxModel = new MaxModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.UpdateMaxes(expectedToken, maxModel)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.UpdateMaxes(maxModel);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion
    }
}