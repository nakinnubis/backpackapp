CREATE TABLE [dbo].[Roles] (
    [role_id]      INT            IDENTITY (1, 1) NOT NULL,
    [role_name_ar] NVARCHAR (MAX) NULL,
    [role_name_en] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([role_id] ASC)
);

