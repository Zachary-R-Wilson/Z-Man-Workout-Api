using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WorkoutApi.Models
{
    public class TrackingModel
    {
        public TrackingModel()
        {
            Exercises = new Dictionary<string, TrackingInfo>();
        }

        public Dictionary<string, TrackingInfo> Exercises { get; set; }
    }

    public class TrackingInfo
    {
        [Required(ErrorMessage = "Date is Required.")]
        public DateTime Date { get; set; }

        public string? Weight { get; set; }

        public int? CompletedReps { get; set; }

        public int? RPE { get; set; }

        [Required(ErrorMessage = "ExerciseKey is Required.")]
        public Guid ExerciseKey { get; set; }
    }

    public class TrackingProgressModel
    {
        public TrackingProgressModel()
        {
            Exercises = new Dictionary<string, TrackingProgress>();
        }

        public Dictionary<string, TrackingProgress> Exercises { get; set; }
    }

    public class TrackingProgress
    {
        public Guid DayKey { get; set; }
        public string DayName { get; set; }
        public Guid ExerciseKey { get; set; }
        public string ExerciseName { get; set; }
        public string Reps { get; set; }
        public int Sets { get; set; }
        public string? Weight { get; set; }
        public int? CompletedReps { get; set; }
        public int? RPE { get; set; }
        public DateTime? Date { get; set; }
    }

    public class AnalysisModel
    {
        public AnalysisModel(TrackingProgress model)
        {
            this.Model = model;
            this.Analysis = GenerateAnalysis();
        }

        public TrackingProgress Model { get; set; }
        public string Analysis { get; set; }

        private string GenerateAnalysis()
        {
            if (Model.RPE.HasValue && Model.RPE < 7)
                return "Low effort, consider increasing intensity.";
            else if (Model.RPE.HasValue && Model.RPE >= 7)
                return "Good effort, maintain or adjust as needed.";
            else
                return "No RPE data available.";
        }
    }
}
