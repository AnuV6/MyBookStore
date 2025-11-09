CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);

-- Insert a user with username 'admin' and password 'admin123'
INSERT INTO Users (Username, PasswordHash)
VALUES (
    'admin',
    'admin123'
);