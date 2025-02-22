CREATE OR ALTER PROCEDURE [dbo].[GetWorkout]
    @WorkoutKey UniqueIdentifier,
	@UserKey UniqueIdentifier
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM Workouts W
        WHERE W.WorkoutKey = @WorkoutKey AND W.UserKey = @UserKey
    )
        THROW 50000, 'Access Denied: You do not have permission to access this workout.', 1;

	SELECT W.Name AS 'Workout', D.Name AS 'Day', E.Name AS Exercise, Reps, [Sets], [Order] FROM [Workouts] W
	JOIN [Days] D ON D.WorkoutKey = W.WorkoutKey
	JOIN Exercises E ON E.DayKey = D.DayKey
	WHERE W.WorkoutKey = @WorkoutKey
	ORDER BY E.[Order] ASC;
END;