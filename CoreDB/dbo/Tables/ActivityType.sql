CREATE TABLE [dbo].[ActivityType] (
    [id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NULL,
    [url]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ActivityType] PRIMARY KEY CLUSTERED ([id] ASC)
);

