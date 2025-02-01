USE [WorkoutDb]
GO
/****** Object:  StoredProcedure [dbo].[GetProgress]    Script Date: 12/8/2024 7:10:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[GetProgress]
	@DayKey Uniqueidentifier,
	@UserKey UniqueIdentifier
AS
BEGIN
	IF NOT EXISTS (
		SELECT 1 
		FROM Workouts W
		JOIN Days D ON D.WorkoutKey = W.WorkoutKey
		WHERE D.DayKey = @DayKey AND W.UserKey = @UserKey
	)
		THROW 50000, 'Access Denied: You do not have permission to access this workout.', 1;


	DECLARE @RecentWorkout TABLE ( ExerciseKey UNIQUEIDENTIFIER, LastWorkout DATETIME );
	INSERT INTO @RecentWorkout (ExerciseKey, LastWorkout) (
		SELECT 
			E.ExerciseKey, MAX(T.LastWorkout) AS LastWorkout
		FROM [DAYS] D
		JOIN Exercises E ON E.DayKey = D.DayKey
		LEFT JOIN [Tracking] T ON T.ExerciseKey = E.ExerciseKey
		WHERE D.DayKey = @DayKey
		GROUP BY E.ExerciseKey
	)

	IF EXISTS (SELECT 1 FROM @RecentWorkout WHERE LastWorkout IS NULL)
		SELECT
			D.[Name] AS 'DayName', 
			E.ExerciseKey, E.[Name] AS 'ExerciseName', E.Reps, E.[Sets],
			NULL [Weight], NULL CompletedReps, NULL RPE,  NULL LastWorkout
		FROM [DAYS] D
		JOIN Exercises E ON E.DayKey = D.DayKey
		LEFT JOIN @RecentWorkout RW ON E.ExerciseKey = RW.ExerciseKey
		WHERE D.DayKey = @DayKey
		ORDER BY E.[Order];
	ELSE
		SELECT
			D.[Name] AS 'DayName', 
			E.ExerciseKey, E.[Name] AS 'ExerciseName', E.Reps, E.[Sets],
			T.[Weight], T.CompletedReps, T.RPE, T.LastWorkout
		FROM [DAYS] D
		JOIN Exercises E ON E.DayKey = D.DayKey
		JOIN [Tracking] T ON T.ExerciseKey = E.ExerciseKey
		JOIN @RecentWorkout RW ON T.LastWorkout = RW.LastWorkout
		WHERE D.DayKey = @DayKey
		ORDER BY E.[Order];
END;