CREATE TABLE [dbo].[Organizer_Type] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [type]        NVARCHAR (MAX) NULL,
    [description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Organizer_Type] PRIMARY KEY CLUSTERED ([id] ASC)
);

