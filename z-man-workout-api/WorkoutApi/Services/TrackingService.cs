using WorkoutApi.Models;
using WorkoutApi.Repositories;

namespace WorkoutApi.Services
{
    public class TrackingService: ITrackingService
    {
        private readonly ITrackingRepository _trackingRepository;
        private readonly IJwtHelper _jwtHelper;

        public TrackingService(ITrackingRepository trackingRepository, IJwtHelper jwtHelper)
        {
            _trackingRepository = trackingRepository;
            _jwtHelper = jwtHelper;
        }


        /// <inheritdoc />
        public TrackingProgressModel GetProgress(string token, Guid dayKey)
        {
            Guid userKey = _jwtHelper.ExtractUserKey(token)
                ?? throw new ArgumentNullException(nameof(token), "Invalid or missing userKey in the token.");

            return _trackingRepository.GetProgress(userKey, dayKey);
        }

        /// <inheritdoc />
        public void InsertTracking(string token, TrackingModel trackingModel)
        {
            Guid userKey = _jwtHelper.ExtractUserKey(token)
                ?? throw new ArgumentNullException(nameof(token), "Invalid or missing userKey in the token.");

            _trackingRepository.InsertTracking(userKey, trackingModel);
        }

        /// <inheritdoc />
        public List<AnalysisModel> GetAnalysis(string token, Guid dayKey)
        {
            Guid userKey = _jwtHelper.ExtractUserKey(token)
                ?? throw new ArgumentNullException(nameof(token), "Invalid or missing userKey in the token.");

            TrackingProgressModel trackingProgressModel = _trackingRepository.GetProgress(userKey, dayKey);

            if (trackingProgressModel == null) return [];

            var analysisList = trackingProgressModel.Exercises
            .Select(kvp => new AnalysisModel(kvp.Value))
            .ToList();

            return analysisList;
        }
    }
}
