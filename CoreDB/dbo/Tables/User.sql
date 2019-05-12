CREATE TABLE [dbo].[User] (
    [id]                     INT            IDENTITY (1, 1) NOT NULL,
    [DOB]                    DATETIME2 (7)  NOT NULL,
    [IBAN_number]            NVARCHAR (MAX) NULL,
    [about_organization]     NVARCHAR (MAX) NULL,
    [address]                NVARCHAR (MAX) NULL,
    [age]                    NVARCHAR (MAX) NULL,
    [bank_id]                INT            NULL,
    [bio]                    NVARCHAR (MAX) NULL,
    [expiry_date]            DATETIME2 (7)  NOT NULL,
    [first_name]             NVARCHAR (MAX) NULL,
    [gender]                 NVARCHAR (MAX) NULL,
    [id_copy]                NVARCHAR (MAX) NULL,
    [identification_number]  NVARCHAR (MAX) NULL,
    [identification_type]    INT            NULL,
    [isProvider]             BIT            NOT NULL,
    [last_name]              NVARCHAR (MAX) NULL,
    [mail]                   NVARCHAR (MAX) NULL,
    [mobile]                 NVARCHAR (MAX) NULL,
    [nationality]            INT            NULL,
    [organization_name]      NVARCHAR (MAX) NULL,
    [organization_type]      NVARCHAR (MAX) NULL,
    [password]               NVARCHAR (MAX) NULL,
    [receive_cash_payment]   BIT            NOT NULL,
    [receive_online_payment] BIT            NOT NULL,
    [recieve_money_transfer] BIT            NOT NULL,
    [user_Type]              INT            NOT NULL,
    [user_name]              NVARCHAR (MAX) NULL,
    [UserPhoto_Url]          NVARCHAR (MAX) NULL,
    [tempUser]               BIT            DEFAULT ((0)) NOT NULL,
    [email] NVARCHAR(255) NULL, 
    [registrationType] NVARCHAR(255) NULL , 
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_User_Banks_bank_id] FOREIGN KEY ([bank_id]) REFERENCES [dbo].[Banks] ([bank_id]),
    CONSTRAINT [FK_User_Identification_types_identification_type] FOREIGN KEY ([identification_type]) REFERENCES [dbo].[Identification_types] ([identification_type_id]),
    CONSTRAINT [FK_User_Nationalities_nationality] FOREIGN KEY ([nationality]) REFERENCES [dbo].[Nationalities] ([nationality_id])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_nationality]
    ON [dbo].[User]([nationality] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_identification_type]
    ON [dbo].[User]([identification_type] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_bank_id]
    ON [dbo].[User]([bank_id] ASC);

