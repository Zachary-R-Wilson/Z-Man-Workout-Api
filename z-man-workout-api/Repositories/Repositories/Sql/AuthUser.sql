CREATE OR ALTER PROCEDURE [dbo].[AuthUser]
    @Email NVARCHAR(254)
AS
BEGIN
    SELECT TOP 1 UserKey, PasswordHash
	FROM Users
	WHERE Email = @Email
END;