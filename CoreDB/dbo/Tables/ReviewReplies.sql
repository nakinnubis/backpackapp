CREATE TABLE [dbo].[ReviewReplies] (
    [replyId]   INT            IDENTITY (1, 1) NOT NULL,
    [userId]    INT            NOT NULL,
    [reply]     NVARCHAR (MAX) NULL,
    [replyTime] DATETIME2 (7)  NOT NULL,
    [review_Id] INT            NOT NULL,
    CONSTRAINT [PK_ReviewReplies] PRIMARY KEY CLUSTERED ([replyId] ASC),
    CONSTRAINT [FK_ReviewReplies_Reviews_review_Id] FOREIGN KEY ([review_Id]) REFERENCES [dbo].[Reviews] ([reviewid]) ON DELETE CASCADE,
    CONSTRAINT [FK_ReviewReplies_User_userId] FOREIGN KEY ([userId]) REFERENCES [dbo].[User] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ReviewReplies_userId]
    ON [dbo].[ReviewReplies]([userId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ReviewReplies_review_Id]
    ON [dbo].[ReviewReplies]([review_Id] ASC);

