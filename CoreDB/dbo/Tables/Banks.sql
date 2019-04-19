CREATE TABLE [dbo].[Banks] (
    [bank_id]      INT            IDENTITY (1, 1) NOT NULL,
    [bank_name_ar] NVARCHAR (MAX) NULL,
    [bank_name_en] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Banks] PRIMARY KEY CLUSTERED ([bank_id] ASC)
);

