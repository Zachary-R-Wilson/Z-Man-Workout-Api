Drop Table IF EXISTS user_profile;

CREATE TABLE user_profile (
    user_id UUID PRIMARY KEY REFERENCES users(user_id) ON DELETE CASCADE,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    date_of_birth DATE,
    gender VARCHAR(10) CHECK (gender IN ('Male', 'Female', 'Other')),
    height_cm INT CHECK (height_cm > 0),
    weight_kg DECIMAL(5,2) CHECK (weight_kg > 0),
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

CREATE TRIGGER trigger_user_profile_updated_at
BEFORE UPDATE ON user_profile
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();