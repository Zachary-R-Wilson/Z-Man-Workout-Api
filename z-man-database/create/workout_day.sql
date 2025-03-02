Drop Table IF EXISTS workout_days;

CREATE TABLE workout_days (
    workout_day_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    workouts_templates_id UUID NOT NULL REFERENCES workout_templates(workout_template_id) ON DELETE CASCADE,
	day_name varchar(225) NOT NULL,
	day_number int NOT NULL,
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

CREATE TRIGGER trigger_workout_days_updated_at
BEFORE UPDATE ON workout_days
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();