CREATE TABLE [dbo].[Booking] (
    [id]              INT             IDENTITY (1, 1) NOT NULL,
    [activity_id]     INT             NULL,
    [avaliability_id] INT             NULL,
    [booking_amount]  DECIMAL (18, 2) NULL,
    [booking_type]    INT             NULL,
    [full_group]      BIT             NOT NULL,
    [is_paid]         BIT             NOT NULL,
    [payment_method]  INT             NOT NULL,
    [user_id]         INT             NULL,
    [customer_id]     INT             NULL,
    [time_option] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Booking_Activity_activity_id] FOREIGN KEY ([activity_id]) REFERENCES [dbo].[Activity] ([id]),
    CONSTRAINT [FK_Booking_Avaliability_avaliability_id] FOREIGN KEY ([avaliability_id]) REFERENCES [dbo].[Avaliability] ([id]),
    CONSTRAINT [FK_Booking_PaymentMethods_payment_method] FOREIGN KEY ([payment_method]) REFERENCES [dbo].[PaymentMethods] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Booking_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_user_id]
    ON [dbo].[Booking]([user_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_payment_method]
    ON [dbo].[Booking]([payment_method] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_avaliability_id]
    ON [dbo].[Booking]([avaliability_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_activity_id]
    ON [dbo].[Booking]([activity_id] ASC);

