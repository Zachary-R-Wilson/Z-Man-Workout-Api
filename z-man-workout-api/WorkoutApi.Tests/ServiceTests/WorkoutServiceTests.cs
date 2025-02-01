using Moq;
using WorkoutApi.Models;
using WorkoutApi.Repositories;
using WorkoutApi.Services;

namespace WorkoutApi.Tests.ServiceTests
{
    public class WorkoutServiceTests
    {
        private readonly WorkoutService _service;
        private readonly Mock<IWorkoutRepository> _mockRepository;
        private readonly Mock<IJwtHelper> _mockJwtHelper;

        public WorkoutServiceTests()
        {
            _mockRepository = new Mock<IWorkoutRepository>();
            _mockJwtHelper = new Mock<IJwtHelper>();
            _service = new WorkoutService(_mockRepository.Object, _mockJwtHelper.Object);
        }

        #region CreateWorkout

        [Fact]
        public void CreateWorkout_ValidToken_CallsRepositoryCreateWorkout()
        {
            // Arrange
            var token = "valid-token";
            var userKey = Guid.NewGuid();
            var workoutModel = new WorkoutModel { Name = "WorkoutName" };

            _mockJwtHelper.Setup(j => j.ExtractUserKey(token)).Returns(userKey);

            // Act
            _service.CreateWorkout(token, workoutModel);

            // Assert
            _mockJwtHelper.Verify(j => j.ExtractUserKey(token), Times.Once);
            _mockRepository.Verify(r => r.CreateWorkout(userKey, workoutModel), Times.Once);
        }

        [Fact]
        public void CreateWorkout_InvalidToken_ThrowsArgumentNullException()
        {
            // Arrange
            var invalidToken = "invalid-token";
            var workoutModel = new WorkoutModel { Name = "WorkoutName" };

            _mockJwtHelper.Setup(j => j.ExtractUserKey(invalidToken)).Returns((Guid?)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.CreateWorkout(invalidToken, workoutModel));
            Assert.Equal("Invalid or missing userKey in the token. (Parameter 'token')", exception.Message);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(invalidToken), Times.Once);
            _mockRepository.Verify(r => r.CreateWorkout(It.IsAny<Guid>(), workoutModel), Times.Never);
        }

        #endregion

        #region DeleteWorkout

        [Fact]
        public void DeleteWorkout_ValidToken_CallsRepositoryDeleteWorkout()
        {
            // Arrange
            var token = "valid-token";
            var userKey = Guid.NewGuid();
            var workoutKey = Guid.NewGuid();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(token)).Returns(userKey);

            // Act
            _service.DeleteWorkout(token, workoutKey);

            // Assert
            _mockJwtHelper.Verify(j => j.ExtractUserKey(token), Times.Once);
            _mockRepository.Verify(r => r.DeleteWorkout(userKey, workoutKey), Times.Once);
        }

        [Fact]
        public void DeleteWorkout_InvalidToken_ThrowsArgumentNullException()
        {
            // Arrange
            var invalidToken = "invalid-token";
            var workoutKey = Guid.NewGuid();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(invalidToken)).Returns((Guid?)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.DeleteWorkout(invalidToken, workoutKey));
            Assert.Equal("Invalid or missing userKey in the token. (Parameter 'token')", exception.Message);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(invalidToken), Times.Once);
            _mockRepository.Verify(r => r.DeleteWorkout(It.IsAny<Guid>(), workoutKey), Times.Never);
        }

        #endregion

        #region GetWorkout

        [Fact]
        public void GetWorkout_ValidToken_ReturnsWorkoutModel()
        {
            // Arrange
            var token = "valid-token";
            var userKey = Guid.NewGuid();
            var workoutKey = Guid.NewGuid();
            var expectedWorkout = new WorkoutModel { Name = "WorkoutName" };

            _mockJwtHelper.Setup(j => j.ExtractUserKey(token)).Returns(userKey);
            _mockRepository.Setup(r => r.GetWorkout(userKey, workoutKey)).Returns(expectedWorkout);

            // Act
            var result = _service.GetWorkout(token, workoutKey);

            // Assert
            Assert.Equal(expectedWorkout, result);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(token), Times.Once);
            _mockRepository.Verify(r => r.GetWorkout(userKey, workoutKey), Times.Once);
        }

        [Fact]
        public void GetWorkout_InvalidToken_ThrowsArgumentNullException()
        {
            // Arrange
            var invalidToken = "invalid-token";
            var workoutKey = Guid.NewGuid();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(invalidToken)).Returns((Guid?)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.GetWorkout(invalidToken, workoutKey));
            Assert.Equal("Invalid or missing userKey in the token. (Parameter 'token')", exception.Message);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(invalidToken), Times.Once);
            _mockRepository.Verify(r => r.GetWorkout(It.IsAny<Guid>(), workoutKey), Times.Never);
        }

        #endregion

        #region GetAllWorkouts

        [Fact]
        public void GetAllWorkouts_ValidToken_ReturnsWorkoutCollection()
        {
            // Arrange
            var token = "valid-token";
            var userKey = Guid.NewGuid();
            var expectedWorkouts = new WorkoutCollection();

            _mockJwtHelper.Setup(j => j.ExtractUserKey(token)).Returns(userKey);
            _mockRepository.Setup(r => r.GetAllWorkouts(userKey)).Returns(expectedWorkouts);

            // Act
            var result = _service.GetAllWorkouts(token);

            // Assert
            Assert.Equal(expectedWorkouts, result);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(token), Times.Once);
            _mockRepository.Verify(r => r.GetAllWorkouts(userKey), Times.Once);
        }

        [Fact]
        public void GetAllWorkouts_InvalidToken_ThrowsArgumentNullException()
        {
            // Arrange
            var invalidToken = "invalid-token";

            _mockJwtHelper.Setup(j => j.ExtractUserKey(invalidToken)).Returns((Guid?)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.GetAllWorkouts(invalidToken));
            Assert.Equal("Invalid or missing userKey in the token. (Parameter 'token')", exception.Message);
            _mockJwtHelper.Verify(j => j.ExtractUserKey(invalidToken), Times.Once);
            _mockRepository.Verify(r => r.GetAllWorkouts(It.IsAny<Guid>()), Times.Never);
        }

        #endregion

    }
}
