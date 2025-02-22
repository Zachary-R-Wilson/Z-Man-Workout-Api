CREATE OR ALTER PROCEDURE [dbo].[VerifyUserExercise]
    @ExerciseKey UniqueIdentifier,
	@UserKey UniqueIdentifier
AS
Begin
    IF NOT EXISTS (
        SELECT 1 FROM Users U
		JOIN Workouts W ON W.UserKey = U.UserKey
		JOIN Days D ON D.WorkoutKey = W.WorkoutKey
		JOIN Exercises E ON E.DayKey = D.DayKey
		WHERE E.ExerciseKey = @ExerciseKey AND U.UserKey = @UserKey
    )
        THROW 50000, 'Access Denied: You do not have permission to access this workout.', 1;
END;