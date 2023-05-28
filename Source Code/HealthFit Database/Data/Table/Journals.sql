
CREATE TABLE [dbo].[Journals]
(
    [JournalID] INT NOT NULL PRIMARY KEY IDENTITY,
    [Title] NVARCHAR(100) NOT NULL,
    [ISSN] NVARCHAR(10) NOT NULL,
    [PublisherID] INT NOT NULL,
    [PublicationFrequency] NVARCHAR(50) NOT NULL,
    [Category] NVARCHAR(50) NOT NULL,
    [PublicationStartYear] NVARCHAR(5) NOT NULL,
    [Description] NVARCHAR(200) NOT NULL,
    [Price] DECIMAL(18, 2) NOT NULL,
    [SubjectArea] NVARCHAR(100),
    [ImpactFactor] NVARCHAR(100),
    [Website] NVARCHAR(100),
    [EditorialBoard] NVARCHAR(100),
    [IndexingInformation] NVARCHAR(100),
    [Format] NVARCHAR(100),
    [CitationMetrics] NVARCHAR(100),
    [SubmissionGuidelines] NVARCHAR(100),
    [Rating] NVARCHAR(5),
    [JournalCoverPhotoPath] NVARCHAR(500),
    [JournalPdfPath] NVARCHAR(500),
    [IsActive] BIT DEFAULT 1,
    FOREIGN KEY ([PublisherID]) REFERENCES Users(UserId)
);