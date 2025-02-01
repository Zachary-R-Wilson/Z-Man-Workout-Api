using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WorkoutApi.Controllers;
using WorkoutApi.Models;
using WorkoutApi.Services;

namespace WorkoutApi.Tests.ControllerTests
{
    public class TrackingControllerTests
    {
        private readonly TrackingController _controller;
        private readonly Mock<ITrackingService> _mockService;

        public TrackingControllerTests()
        {
            _mockService = new Mock<ITrackingService>();
            _controller = new TrackingController(_mockService.Object);
        }

        #region GetProgress

        [Fact]
        public void GetProgress_EmptyDayKey_ReturnsBadRequest()
        {
            // Arrange
            var dayKey = Guid.Empty;

            // Act
            var result = _controller.GetProgress(dayKey);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetProgress_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            var dayKey = Guid.NewGuid();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.GetProgress(dayKey);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void GetProgress_ValidRequest_ReturnsOk()
        {
            // Arrange
            var expectedToken = "valid-token";
            var dayKey = Guid.NewGuid();
            var expectedProgress = new TrackingProgressModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.GetProgress(expectedToken, dayKey)).Returns(expectedProgress);

            // Act
            var result = _controller.GetProgress(dayKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedProgress, okResult.Value);
        }


        [Fact]
        public void GetProgress_ServiceThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var expectedToken = "valid-token";
            var dayKey = Guid.NewGuid();
            var expectedProgress = new TrackingProgressModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.GetProgress(expectedToken, dayKey)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.GetProgress(dayKey);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion


        #region InsertTracking

        [Fact]
        public void InsertTracking_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Date", "Required");

            // Act
            var result = _controller.InsertTracking(new TrackingModel());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void InsertTracking_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.InsertTracking(new TrackingModel());

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void InsertTracking_ValidRequest_ReturnsOk()
        {
            // Arrange
            var expectedToken = "valid-token";
            var trackingModel = new TrackingModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            // Act
            var result = _controller.InsertTracking(trackingModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Progress Successfully Saved", okResult.Value);
        }


        [Fact]
        public void InsertTracking_ServiceThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var expectedToken = "valid-token";
            var trackingModel = new TrackingModel();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.InsertTracking(expectedToken, trackingModel)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.InsertTracking(trackingModel);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion

        #region GetAnalysis

        [Fact]
        public void GetAnalysis_EmptyDayKey_ReturnsBadRequest()
        {
            // Arrange
            var dayKey = Guid.Empty;

            // Act
            var result = _controller.GetAnalysis(dayKey);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetAnalysis_MissingAuthorizationHeader_ReturnsUnauthorized()
        {
            // Arrange
            var dayKey = Guid.NewGuid();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.GetAnalysis(dayKey);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Missing or invalid Authorization header.", unauthorizedResult.Value);
        }

        [Fact]
        public void GetAnalysis_ValidRequest_ReturnsOk()
        {
            // Arrange
            var expectedToken = "valid-token";
            var dayKey = Guid.NewGuid();
            var expectedAnalysis = new List<AnalysisModel>();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.GetAnalysis(expectedToken, dayKey)).Returns(expectedAnalysis);

            // Act
            var result = _controller.GetAnalysis(dayKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedAnalysis, okResult.Value);
        }


        [Fact]
        public void GetAnalysis_ServiceThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var expectedToken = "valid-token";
            var dayKey = Guid.NewGuid();
            var expectedAnalysis = new List<AnalysisModel>();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.Request.Headers["Authorization"] = $"Bearer {expectedToken}";

            _mockService.Setup(service => service.GetAnalysis(expectedToken, dayKey)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.GetAnalysis(dayKey);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        #endregion
    }
}