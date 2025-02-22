CREATE OR ALTER PROCEDURE [dbo].[CreateWorkout]
	@UserKey Uniqueidentifier,
    @WorkoutName NVARCHAR(256)
AS
BEGIN
    DECLARE @WorkoutKey uniqueidentifier = NEWID();

	INSERT INTO [Workouts]
	VALUES (@WorkoutKey, @WorkoutName, @UserKey);

	SELECT @WorkoutKey;
END;