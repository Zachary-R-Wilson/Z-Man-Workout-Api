CREATE OR ALTER PROCEDURE [dbo].[GetAllWorkouts]
    @UserKey UniqueIdentifier
AS
BEGIN
	SELECT W.WorkoutKey, W.Name AS 'Workout', D.DayKey, D.Name AS 'Day' FROM [Workouts] W
	JOIN [Days] D ON D.WorkoutKey = W.WorkoutKey
	JOIN [Exercises] E ON E.DayKey = D.DayKey
	LEFT JOIN [Tracking] T ON T.ExerciseKey = E.ExerciseKey
	WHERE W.UserKey = @UserKey
	GROUP BY D.DayKey, D.Name, W.WorkoutKey, W.Name
	ORDER BY ISNULL(MAX(T.LastWorkout), '2000-01-01') ASC;
END;