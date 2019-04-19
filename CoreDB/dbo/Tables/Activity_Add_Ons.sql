CREATE TABLE [dbo].[Activity_Add_Ons] (
    [id]                INT             IDENTITY (1, 1) NOT NULL,
    [activity_id]       INT             NULL,
    [add_ons_id]        INT             NULL,
    [addons_number]     INT             NOT NULL,
    [note]              NVARCHAR (MAX)  NULL,
    [price]             DECIMAL (18, 2) NULL,
    [provider_Username] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Activity_Add_Ons] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Activity_Add_Ons_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id]),
    CONSTRAINT [FK_Activity_Add_Ons_Add_Ons_add_ons_id] FOREIGN KEY ([add_ons_id]) REFERENCES [dbo].[Add_Ons] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Add_Ons_add_ons_id]
    ON [dbo].[Activity_Add_Ons]([add_ons_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Add_Ons_activity_id]
    ON [dbo].[Activity_Add_Ons]([activity_id] ASC);

