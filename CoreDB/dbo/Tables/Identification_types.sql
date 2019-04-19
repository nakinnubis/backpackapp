CREATE TABLE [dbo].[Identification_types] (
    [identification_type_id] INT            IDENTITY (1, 1) NOT NULL,
    [identification_type_ar] NVARCHAR (MAX) NULL,
    [identification_type_en] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Identification_types] PRIMARY KEY CLUSTERED ([identification_type_id] ASC)
);

