using Moq;
using WorkoutApi.Models;
using WorkoutApi.Repositories;
using WorkoutApi.Services;

namespace WorkoutApi.Tests.ServiceTests
{
    public class MaxesServiceTests
    {
        private readonly MaxesService _service;
        private readonly Mock<IMaxesRepository> _mockRepository;
        private readonly Mock<IJwtHelper> _mockJwtHelper;

        public MaxesServiceTests()
        {
            _mockRepository = new Mock<IMaxesRepository>();
            _mockJwtHelper = new Mock<IJwtHelper>();
            _service = new MaxesService(_mockRepository.Object, _mockJwtHelper.Object);
        }

        #region GetMaxes

        [Fact]
        public void GetMaxes_ValidToken_ReturnsMaxModel()
        {
            // Arrange
            var token = "valid-token";
            var userKey = Guid.NewGuid();
            var expectedMaxModel = new MaxModel();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(token)).Returns(userKey);
            _mockRepository.Setup(r => r.GetMaxes(userKey)).Returns(expectedMaxModel);

            // Act
            var result = _service.GetMaxes(token);

            // Assert
            Assert.Equal(expectedMaxModel, result);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(token), Times.Once);
            _mockRepository.Verify(r => r.GetMaxes(userKey), Times.Once);
        }

        [Fact]
        public void GetMaxes_InvalidToken_ThrowsArgumentNullException()
        {
            // Arrange
            var invalidToken = "invalid-token";
            _mockJwtHelper.Setup(j => j.ExtractUserKey(invalidToken)).Returns((Guid?)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.GetMaxes(invalidToken));
            Assert.Equal("Invalid or missing userKey in the token. (Parameter 'token')", exception.Message);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(invalidToken), Times.Once);
            _mockRepository.Verify(r => r.GetMaxes(It.IsAny<Guid>()), Times.Never);
        }

        #endregion

        #region UpdateMaxes

        [Fact]
        public void UpdateMaxes_ValidToken_CallsRepositoryUpdateMaxes()
        {
            // Arrange
            var token = "valid-token";
            var userKey = Guid.NewGuid();
            var trackingModel = new MaxModel();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(token)).Returns(userKey);
            _mockRepository.Setup(r => r.UpdateMaxes(userKey, trackingModel)).Verifiable();

            // Act
            _service.UpdateMaxes(token, trackingModel);

            // Assert
            _mockJwtHelper.Verify(j => j.ExtractUserKey(token), Times.Once);
            _mockRepository.Verify(r => r.UpdateMaxes(userKey, trackingModel), Times.Once);
        }

        [Fact]
        public void UpdateMaxes_InvalidToken_ThrowsArgumentNullException()
        {
            // Arrange
            var invalidToken = "invalid-token";
            var trackingModel = new MaxModel();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(invalidToken)).Returns((Guid?)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.UpdateMaxes(invalidToken, trackingModel));
            Assert.Equal("Invalid or missing userKey in the token. (Parameter 'token')", exception.Message);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(invalidToken), Times.Once);
            _mockRepository.Verify(r => r.UpdateMaxes(It.IsAny<Guid>(), trackingModel), Times.Never);
        }

        #endregion
    }
}
