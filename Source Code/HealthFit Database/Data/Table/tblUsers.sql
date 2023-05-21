CREATE TABLE [dbo].[tblUsers]
(
	[UserId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FullName] NVARCHAR(100) NOT NULL, 
    [Address] NVARCHAR(200) NOT NULL, 
    [City] NVARCHAR(50) NULL, 
    [State] SMALLINT NOT NULL DEFAULT 0, 
    [Country] SMALLINT NOT NULL DEFAULT 0, 
    [Email] NVARCHAR(100) NOT NULL, 
    [PhoneNo] NCHAR(15) NULL, 
    [Website] NVARCHAR(100) NULL, 
    [UserType] TINYINT NOT NULL, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [HashedPassword] NVARCHAR(300) NOT NULL, 
    [PasswordSalt] NVARCHAR(300) NOT NULL, 
    [IsActive] BIT NULL DEFAULT 1
)
