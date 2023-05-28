CREATE TABLE [dbo].[Users] (
    UserId INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(50) NOT NULL,
    Address NVARCHAR(300),
    City NVARCHAR(50),
    State NVARCHAR(50),
    Country NVARCHAR(50),
    Email NVARCHAR(100) NOT NULL,
    PhoneNo NVARCHAR(15),
    Website NVARCHAR(100),
    UserType TINYINT NOT NULL,
    UserName NVARCHAR(50) NOT NULL,
    HashedPassword NVARCHAR(100),
    PasswordSalt NVARCHAR(100),
    IsActive BIT NOT NULL
);
