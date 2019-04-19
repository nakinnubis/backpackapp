CREATE TABLE [dbo].[Activity_Log] (
    [activity_log_id] INT            IDENTITY (1, 1) NOT NULL,
    [action]          NVARCHAR (MAX) NULL,
    [activity_id]     INT            NOT NULL,
    [log_date]        DATETIME2 (7)  NOT NULL,
    [user_id]         INT            NOT NULL,
    CONSTRAINT [PK_Activity_Log] PRIMARY KEY CLUSTERED ([activity_log_id] ASC),
    CONSTRAINT [FK_Activity_Log_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Log_user_id]
    ON [dbo].[Activity_Log]([user_id] ASC);

