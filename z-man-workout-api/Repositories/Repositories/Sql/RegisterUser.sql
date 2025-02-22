CREATE OR ALTER PROCEDURE [dbo].[RegisterUser]
	@Email NVARCHAR(254),
	@PasswordHash NVARCHAR(75)
AS
BEGIN
	IF EXISTS (
			SELECT 1
			FROM Users
			WHERE Email = @Email
		)
		THROW 50000, 'An account already exists for this email.', 1;

	DECLARE @userKey uniqueidentifier = NEWID();

	INSERT INTO [Users]
	VALUES (@userKey, @Email, @PasswordHash);

	SELECT @userKey;
END;