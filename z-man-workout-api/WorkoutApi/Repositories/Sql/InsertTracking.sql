CREATE OR ALTER PROCEDURE [dbo].[InsertTracking]
	@ExerciseKey Uniqueidentifier,
	@UserKey Uniqueidentifier,
    @Date Date,
	@Weight NVARCHAR(256),
	@CompletedReps INTEGER,
	@RPE INTEGER
AS
BEGIN
	EXEC [VerifyUserExercise] @ExerciseKey, @UserKey;

	INSERT INTO [Tracking] 
	VALUES (@Date, @Weight, @CompletedReps, @RPE, NULL, @ExerciseKey);
END;