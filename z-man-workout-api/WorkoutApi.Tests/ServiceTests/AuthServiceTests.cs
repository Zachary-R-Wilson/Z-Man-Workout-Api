using Moq;
using WorkoutApi.Models;
using WorkoutApi.Repositories;
using WorkoutApi.Services;

namespace WorkoutApi.Tests.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly AuthService _service;
        private readonly Mock<IAuthRepository> _mockRepository;
        private readonly Mock<IJwtHelper> _mockJwtHelper;

        public AuthServiceTests()
        {
            _mockRepository = new Mock<IAuthRepository>();
            _mockJwtHelper = new Mock<IJwtHelper>();
            _service = new AuthService(_mockRepository.Object, _mockJwtHelper.Object);
        }

        #region AuthenticateUser

        [Fact]
        public void AuthenticateUser_ValidCredentials_ReturnsJwt()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "testing@email.com", Password = "password" };
            var userKey = Guid.NewGuid();
            var expectedToken = "valid_token";

            _mockRepository.Setup(x => x.AuthenticateUser(loginModel)).Returns(userKey);

            _mockJwtHelper.Setup(x => x.GenerateAccessToken(userKey)).Returns(expectedToken);

            // Act
            var result = _service.AuthenticateUser(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result);
        }

        [Fact]
        public void AuthenticateUser_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "invalid@email.com", Password = "password" };

            _mockRepository.Setup(x => x.AuthenticateUser(loginModel)).Returns((Guid?)null);

            // Act
            var result = _service.AuthenticateUser(loginModel);

            // Assert
            Assert.Null(result);
        }


        #endregion

        #region RegisterUser

        [Fact]
        public void RegisterUser_ValidCredentials_ReturnsJwt()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "testing@email.com", Password = "password" };
            var userKey = Guid.NewGuid();
            var expectedToken = "valid_token";

            _mockRepository.Setup(x => x.AuthenticateUser(loginModel)).Returns(userKey);

            _mockJwtHelper.Setup(x => x.GenerateAccessToken(userKey)).Returns(expectedToken);

            // Act
            var result = _service.AuthenticateUser(loginModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedToken, result);
        }

        [Fact]
        public void RegisterUser_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var loginModel = new LoginModel { Email = "invalid@email.com", Password = "password" };

            _mockRepository.Setup(x => x.AuthenticateUser(loginModel)).Returns((Guid?)null);

            // Act
            var result = _service.AuthenticateUser(loginModel);

            // Assert
            Assert.Null(result);
        }

        #endregion
    }
}
