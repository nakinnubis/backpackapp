CREATE TABLE [dbo].[ReviewReports] (
    [reportId]   INT           IDENTITY (1, 1) NOT NULL,
    [reviewId]   INT           NOT NULL,
    [reportDate] DATETIME2 (7) NOT NULL,
    [user_id]    INT           NULL,
    CONSTRAINT [PK_ReviewReports] PRIMARY KEY CLUSTERED ([reportId] ASC),
    CONSTRAINT [FK_ReviewReports_Reviews_reviewId] FOREIGN KEY ([reviewId]) REFERENCES [dbo].[Reviews] ([reviewid]) ON DELETE CASCADE,
    CONSTRAINT [FK_ReviewReports_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ReviewReports_user_id]
    ON [dbo].[ReviewReports]([user_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReviewReports_reviewId]
    ON [dbo].[ReviewReports]([reviewId] ASC);

