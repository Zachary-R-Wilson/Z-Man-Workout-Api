CREATE OR ALTER PROCEDURE [dbo].[DeleteWorkout]
	@WorkoutKey Uniqueidentifier,
	@UserKey UniqueIdentifier
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM Workouts W
        WHERE W.WorkoutKey = @WorkoutKey AND W.UserKey = @UserKey
    )
        THROW 50000, 'Access Denied: You do not have permission to delete this workout.', 1;

    Delete FROM Workouts where WorkoutKey = @WorkoutKey;
END;