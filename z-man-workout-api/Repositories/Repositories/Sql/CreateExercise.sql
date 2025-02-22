CREATE OR ALTER PROCEDURE [dbo].[CreateExercise]
	@DayKey Uniqueidentifier,
    @ExerciseName NVARCHAR(256),
	@ExerciseReps NVARCHAR(256),
	@ExerciseSets INTEGER,
	@Order INTEGER
AS
BEGIN
    DECLARE @ExerciseKey uniqueidentifier = NEWID();

	INSERT INTO [Exercises]
	VALUES (@ExerciseKey, @ExerciseName, @ExerciseReps, @ExerciseSets, @Order, @DayKey);

	SELECT @DayKey;
END;