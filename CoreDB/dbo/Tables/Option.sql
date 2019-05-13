CREATE TABLE [dbo].[Option] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [description] NVARCHAR (MAX) NULL,
    [icon]        NVARCHAR (MAX) NULL,
    [name]        NVARCHAR (MAX) NULL,
    [haveAge]     BIT            DEFAULT ((0)) NOT NULL,
    [fromAge] INT NULL, 
    [toAge] INT NULL, 
    [_class] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Option] PRIMARY KEY CLUSTERED ([id] ASC)
);

