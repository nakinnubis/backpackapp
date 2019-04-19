CREATE TABLE [dbo].[Activity_Rule] (
    [id]          INT IDENTITY (1, 1) NOT NULL,
    [activity_id] INT NULL,
    [rule_id]     INT NULL,
    CONSTRAINT [PK_Activity_Rule] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Activity_Rule_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id]),
    CONSTRAINT [FK_Activity_Rule_Rule_rule_id] FOREIGN KEY ([rule_id]) REFERENCES [dbo].[Rule] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Rule_rule_id]
    ON [dbo].[Activity_Rule]([rule_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_Rule_activity_id]
    ON [dbo].[Activity_Rule]([activity_id] ASC);

