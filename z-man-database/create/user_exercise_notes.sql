Drop Table IF EXISTS user_exercise_notes;

CREATE TABLE user_exercise_notes (
    note_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    tracking_id UUID NOT NULL REFERENCES user_exercise_tracking(tracking_id) ON DELETE CASCADE,
	note TEXT,
	created_at TIMESTAMPTZ DEFAULT now()
);
