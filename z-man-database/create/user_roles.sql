Drop Table IF EXISTS user_roles;

CREATE TABLE user_roles (
    user_role_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_role VARCHAR(10) CHECK (user_role IN ('user', 'coach', 'admin'))
);