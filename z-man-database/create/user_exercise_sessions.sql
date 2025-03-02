Drop Table IF EXISTS user_exercise_sessions;

CREATE TABLE user_exercise_sessions (
    session_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
	user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    user_workout_id UUID NOT NULL REFERENCES user_workouts(user_workout_id) ON DELETE CASCADE,
	started_at TIMESTAMPTZ DEFAULT now(),
	completed_at TIMESTAMPTZ DEFAULT now()
);
