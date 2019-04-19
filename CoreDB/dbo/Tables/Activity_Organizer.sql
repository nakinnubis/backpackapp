CREATE TABLE [dbo].[Activity_Organizer] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [Organizer_Typeid] INT            NULL,
    [activity_id]      INT            NULL,
    [mail]             NVARCHAR (MAX) NULL,
    [mobile]           NVARCHAR (MAX) NULL,
    [name]             NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Activity_Organizer] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Activity_Organizer_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id]),
    CONSTRAINT [FK_Activity_Organizer_Organizer_Type_Organizer_Typeid] FOREIGN KEY ([Organizer_Typeid]) REFERENCES [dbo].[Organizer_Type] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Organizer_Organizer_Typeid]
    ON [dbo].[Activity_Organizer]([Organizer_Typeid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Organizer_activity_id]
    ON [dbo].[Activity_Organizer]([activity_id] ASC);

