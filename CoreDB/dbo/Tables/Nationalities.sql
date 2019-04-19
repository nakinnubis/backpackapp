CREATE TABLE [dbo].[Nationalities] (
    [nationality_id]      INT            IDENTITY (1, 1) NOT NULL,
    [nationality_name_ar] NVARCHAR (MAX) NULL,
    [nationality_name_en] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Nationalities] PRIMARY KEY CLUSTERED ([nationality_id] ASC)
);

