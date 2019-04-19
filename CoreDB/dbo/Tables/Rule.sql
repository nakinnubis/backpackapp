CREATE TABLE [dbo].[Rule] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [add_by]      INT            NOT NULL,
    [description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Rule_User_add_by] FOREIGN KEY ([add_by]) REFERENCES [dbo].[User] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Rule_add_by]
    ON [dbo].[Rule]([add_by] ASC);

