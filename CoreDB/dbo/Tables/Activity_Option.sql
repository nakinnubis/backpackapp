CREATE TABLE [dbo].[Activity_Option] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [activity_id] INT            NULL,
    [option_id]   INT            NULL,
    [fromAge]     NVARCHAR (MAX) NULL,
    [toAge]       NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Activity_Option] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Activity_Option_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id]),
    CONSTRAINT [FK_Activity_Option_Option_option_id] FOREIGN KEY ([option_id]) REFERENCES [dbo].[Option] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Option_option_id]
    ON [dbo].[Activity_Option]([option_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Option_activity_id]
    ON [dbo].[Activity_Option]([activity_id] ASC);

