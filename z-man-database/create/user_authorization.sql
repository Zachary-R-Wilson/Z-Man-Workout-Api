Drop Table IF EXISTS user_authorization;

CREATE TABLE user_authorization (
    user_role_id UUID NOT NULL REFERENCES user_roles(user_role_id) ON DELETE CASCADE,
    user_id UUID NOT NULL REFERENCES users(user_id) ON DELETE CASCADE
);