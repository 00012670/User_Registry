CREATE TABLE [User] (
    UserId INT PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50),
    LastLogin DATETIME,
    Status INT
);

EXEC sp_rename 'User', 'Users';