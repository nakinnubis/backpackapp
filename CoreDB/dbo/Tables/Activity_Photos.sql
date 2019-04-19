CREATE TABLE [dbo].[Activity_Photos] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [activity_id] INT            NULL,
    [cover_photo] BIT            NOT NULL,
    [url]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Activity_Photos] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Activity_Photos_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Photos_activity_id]
    ON [dbo].[Activity_Photos]([activity_id] ASC);

