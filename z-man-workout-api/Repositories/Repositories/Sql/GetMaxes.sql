CREATE OR ALTER PROCEDURE [dbo].[GetMaxes]
    @UserKey UniqueIdentifier
AS
BEGIN
	SELECT M.Squat, M.Deadlift, M.Benchpress FROM [Maxes] M
	WHERE M.UserKey = @UserKey;
END;