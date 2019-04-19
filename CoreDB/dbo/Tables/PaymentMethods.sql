CREATE TABLE [dbo].[PaymentMethods] (
    [id]   INT            IDENTITY (1, 1) NOT NULL,
    [name] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED ([id] ASC)
);

