Drop Table IF EXISTS user_exercise_tracking;

CREATE TABLE user_exercise_tracking (
    tracking_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    session_id UUID NOT NULL REFERENCES user_exercise_sessions(session_id) ON DELETE CASCADE,
	exercise_name VARCHAR(255) NOT NULL,
	set_number INT,
	reps INT,
	weight DECIMAL(5,2),
	duration_seconds INT, -- Time in seconds (cardio)
	distance DECIMAL(5,2) -- Distance in km (cardio)
);
