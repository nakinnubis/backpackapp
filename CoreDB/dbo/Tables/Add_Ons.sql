CREATE TABLE [dbo].[Add_Ons] (
    [id]   INT            IDENTITY (1, 1) NOT NULL,
    [name] NVARCHAR (MAX) NULL,
    [icon] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Add_Ons] PRIMARY KEY CLUSTERED ([id] ASC)
);

