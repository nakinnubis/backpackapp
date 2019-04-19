CREATE TABLE [dbo].[Individual_Categories] (
    [id]                   INT             IDENTITY (1, 1) NOT NULL,
    [activityid]           INT             NOT NULL,
    [capacity]             INT             NULL,
    [name]                 NVARCHAR (MAX)  NULL,
    [price]                DECIMAL (18, 2) NOT NULL,
    [price_after_discount] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Individual_Categories] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Individual_Categories_Activity_activityid] FOREIGN KEY ([activityid]) REFERENCES [dbo].[Activity] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Individual_Categories_activityid]
    ON [dbo].[Individual_Categories]([activityid] ASC);

