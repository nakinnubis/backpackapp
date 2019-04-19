CREATE TABLE [dbo].[Notifications] (
    [notification_id]   INT            IDENTITY (1, 1) NOT NULL,
    [notification_desc] NVARCHAR (MAX) NULL,
    [user_id]           INT            NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([notification_id] ASC),
    CONSTRAINT [FK_Notifications_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Notifications_user_id]
    ON [dbo].[Notifications]([user_id] ASC);

