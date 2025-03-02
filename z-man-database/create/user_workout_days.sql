Drop Table IF EXISTS user_workout_days;

CREATE TABLE user_workout_days (
    user_workout_day_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_workout_id UUID NOT NULL REFERENCES user_workouts(user_workout_id) ON DELETE CASCADE,
	custom_day_name VARCHAR(225) NOT NULL,
	day_number INT NOT NULL,
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

CREATE TRIGGER trigger_user_workout_days_updated_at
BEFORE UPDATE ON user_workout_days
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();