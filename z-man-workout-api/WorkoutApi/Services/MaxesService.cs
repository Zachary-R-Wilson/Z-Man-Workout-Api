using System.IdentityModel.Tokens.Jwt;
using WorkoutApi.Models;
using WorkoutApi.Repositories;

namespace WorkoutApi.Services
{
    public class MaxesService : IMaxesService
    {
        private readonly IMaxesRepository _maxesRepository;
        private readonly IJwtHelper _jwtHelper;

        public MaxesService(IMaxesRepository maxesRepository, IJwtHelper jwtHelper)
        {
            _maxesRepository = maxesRepository;
            _jwtHelper = jwtHelper;
        }

        /// <inheritdoc />
        public MaxModel GetMaxes(string token)
        {
            Guid userKey = _jwtHelper.ExtractUserKey(token)
                ?? throw new ArgumentNullException(nameof(token), "Invalid or missing userKey in the token.");
            return _maxesRepository.GetMaxes(userKey);
        }

        /// <inheritdoc />
        public void UpdateMaxes(string token, MaxModel trackingModel)
        {
            Guid userKey = _jwtHelper.ExtractUserKey(token)
                ?? throw new ArgumentNullException(nameof(token), "Invalid or missing userKey in the token.");
            _maxesRepository.UpdateMaxes(userKey, trackingModel);
        }
    }
}
