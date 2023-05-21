CREATE TABLE [dbo].[tblJournals]
(
	[JournalId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(100) NOT NULL, 
    [ISSN] NVARCHAR(10) NOT NULL, 
    [PublisherId] INT NOT NULL ,
    [PublicationFrequency] INT NOT NULL, 
    [PublicationStartYear] NCHAR(5) NULL, 
    [SubjectArea] NVARCHAR(100) NULL, 
    [ImpactFactor] NVARCHAR(100) NULL, 
    [Website] NVARCHAR(50) NULL, 
    [EditorialBoard] NVARCHAR(100) NULL, 
    [Description] NCHAR(10) NULL, 
    [JournalCoverPhotoName] NVARCHAR(50) NULL, 
    [JournalCoverPhotoPath] NVARCHAR(100) NULL, 
    [JournalPdfName] NVARCHAR(50) NULL, 
    [JournalPdfPath] NVARCHAR(100) NULL, 
    [IsActive] BIT NULL DEFAULT 1, 
    FOREIGN KEY ([PublisherId]) REFERENCES tblUsers(UserId)

)
