Drop Table IF EXISTS user_oauth;

CREATE TABLE user_oauth (
    user_oauth_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE,
    provider VARCHAR(50) NOT NULL,  -- (e.g., 'google', 'github', 'facebook')
    provider_user_id VARCHAR(255) NOT NULL,  -- User's unique ID from OAuth provider
    access_token TEXT NOT NULL,  -- OAuth access token (store securely!)
    refresh_token TEXT,  -- Optional, used for long-term sessions
    expires_at TIMESTAMPTZ,  -- Token expiration timestamp
    created_at TIMESTAMPTZ DEFAULT now(),
    updated_at TIMESTAMPTZ DEFAULT now(),
    UNIQUE(user_id, provider)  -- Ensures each user only has one entry per provider
);

CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = now();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_user_oauth_updated_at
BEFORE UPDATE ON user_oauth
FOR EACH ROW
EXECUTE FUNCTION update_updated_at_column();