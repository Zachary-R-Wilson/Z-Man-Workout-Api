Drop Table IF EXISTS user_workouts;

CREATE TABLE user_workouts (
    user_workout_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    workouts_templates_id UUID NOT NULL REFERENCES workout_templates(workout_template_id) ON DELETE CASCADE,
	custom_name varchar(225),	
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

CREATE TRIGGER trigger_user_workouts_updated_at
BEFORE UPDATE ON user_workouts
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();