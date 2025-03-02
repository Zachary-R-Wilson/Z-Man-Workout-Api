Drop Table IF EXISTS workout_templates;

CREATE TABLE workout_templates (
    workout_template_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    created_by UUID REFERENCES users(user_id) ON DELETE CASCADE,
	name varchar(225) NOT NULL,
	description text NOT NULL,
	public bool DEFAULT FALSE,
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

CREATE TRIGGER trigger_workout_templates_updated_at
BEFORE UPDATE ON workout_templates
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();