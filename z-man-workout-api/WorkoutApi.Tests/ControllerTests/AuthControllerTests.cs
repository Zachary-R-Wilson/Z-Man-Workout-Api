using Moq;
using Microsoft.AspNetCore.Mvc;
using WorkoutApi.Models;
using WorkoutApi.Controllers;
using WorkoutApi.Services;
using System.ComponentModel.DataAnnotations;

namespace WorkoutApi.Tests.ControllerTests
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _mockService;

        public AuthControllerTests()
        {
            _mockService = new Mock<IAuthService>();
            _controller = new AuthController(_mockService.Object);
        }

        #region LoginTests

        [Fact]
        public void Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "testing@email.com", Password = "password" };
            _mockService.Setup(x => x.AuthenticateUser(loginModel)).Returns("valid_token");

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.Equal("{ AccessToken = valid_token }", response?.ToString());
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "invalid@email.com", Password = "invalidPassword" };
            _mockService.Setup(x => x.AuthenticateUser(loginModel)).Returns((string)null);

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Login_InvalidLoginModel_ReturnsBadRequest()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "", Password = null };

            // Simulate model validation of dataModel
            var validationContext = new ValidationContext(loginModel);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(loginModel, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                _controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_ThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "testing@email.com", Password = "password" };
            _mockService.Setup(x => x.AuthenticateUser(loginModel)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.Login(loginModel);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        #endregion

        #region RegisterTests

        [Fact]
        public void Register_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "newTesting@email.com", Password = "password" };
            _mockService.Setup(x => x.RegisterUser(loginModel)).Returns("valid_token");

            // Act
            var result = _controller.Register(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value as dynamic;
            Assert.Equal("{ AccessToken = valid_token }", response?.ToString());
        }

        [Fact]
        public void Register_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "newInvalid@email.com", Password = "invalidPassword" };
            _mockService.Setup(x => x.RegisterUser(loginModel)).Returns((string)null);

            // Act
            var result = _controller.Register(loginModel);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Register_InvalidLoginModel_ReturnsBadRequest()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "", Password = null };

            // Simulate model validation of dataModel
            var validationContext = new ValidationContext(loginModel);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(loginModel, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                _controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            // Act
            var result = _controller.Register(loginModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Register_ThrowsSqlException_ReturnsInternalServerError()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "testing@email.com", Password = "password" };
            _mockService.Setup(x => x.RegisterUser(loginModel)).Throws(SqlExceptionHelper.MakeSqlException());

            // Act
            var result = _controller.Register(loginModel);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        #endregion
    }
}
