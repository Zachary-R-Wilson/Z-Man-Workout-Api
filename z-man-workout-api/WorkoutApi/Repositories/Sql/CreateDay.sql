CREATE OR ALTER PROCEDURE [dbo].[CreateDay]
	@WorkoutKey Uniqueidentifier,
    @DayName NVARCHAR(256)
AS
BEGIN
    DECLARE @DayKey uniqueidentifier = NEWID();

	INSERT INTO [Days]
	VALUES (@DayKey, @DayName, @WorkoutKey);

	SELECT @DayKey;
END;