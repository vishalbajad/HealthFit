CREATE TABLE [dbo].[UserSubscriptionsDetails]
(
    [Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [JournalId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [SubscriptionStartDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [SubscriptionEndDate] DATETIME NOT NULL,
    FOREIGN KEY ([JournalId]) REFERENCES [dbo].[Journals]([JournalID]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId])
);