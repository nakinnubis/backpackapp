CREATE TABLE [dbo].[Reviews] (
    [reviewid]    INT             IDENTITY (1, 1) NOT NULL,
    [activity_id] INT             NULL,
    [date]        DATETIME2 (7)   NULL,
    [rate]        DECIMAL (18, 2) NULL,
    [review]      NVARCHAR (MAX)  NULL,
    [user_id]     INT             NULL,
    [isBlocked]   BIT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED ([reviewid] ASC),
    CONSTRAINT [FK_Reviews_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id]),
    CONSTRAINT [FK_Reviews_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Reviews_user_id]
    ON [dbo].[Reviews]([user_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Reviews_activity_id]
    ON [dbo].[Reviews]([activity_id] ASC);

