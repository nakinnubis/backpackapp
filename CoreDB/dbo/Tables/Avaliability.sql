CREATE TABLE [dbo].[Avaliability] (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [isForGroup]     INT             NULL,
    [activity_End]   DATETIME2 (7)   NULL,
    [activity_Start] DATETIME2 (7)   NULL,
    [activity_id]    INT             NULL,
    [total_tickets]  INT             NULL,
    [group_Price]    DECIMAL (18, 2) NULL,
    [reoccuring] BIT NULL, 
    [isForIndividual] INT NULL, 
    CONSTRAINT [PK_Avaliability] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Avaliability_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Avaliability_activity_id]
    ON [dbo].[Avaliability]([activity_id] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'return 1 in case of group, 0 for individual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Avaliability', @level2type = N'COLUMN', @level2name = N'isForGroup';

