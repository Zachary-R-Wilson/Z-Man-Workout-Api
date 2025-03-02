Drop Table IF EXISTS user_workout_exercises;

CREATE TABLE user_workout_exercises (
    user_workout_exercise_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_workout_day_id UUID NOT NULL REFERENCES user_workout_day(user_workout_day_id) ON DELETE CASCADE,
	exercise_name varchar(225) NOT NULL,
	exercise_number int NOT NULL,
	sets int NOT NULL,
	reps varchar(225) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT now(),
    updated_at TIMESTAMPTZ DEFAULT now()
);

CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_user_workout_exercises_updated_at
BEFORE UPDATE ON user_workout_exercises
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();