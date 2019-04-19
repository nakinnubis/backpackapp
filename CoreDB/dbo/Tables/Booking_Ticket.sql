CREATE TABLE [dbo].[Booking_Ticket] (
    [ticket_id]         INT            IDENTITY (1, 1) NOT NULL,
    [booking_id]        INT            NULL,
    [mail]              NVARCHAR (MAX) NULL,
    [mobile]            NVARCHAR (MAX) NULL,
    [name]              NVARCHAR (MAX) NULL,
    [ticket_cancelled]  BIT            NOT NULL,
    [ticket_checked_in] BIT            NOT NULL,
    [ticket_number]     BIGINT         NULL,
    [ticket_reviewd]    BIT            NOT NULL,
    [primaryTicket]     BIT            DEFAULT ((0)) NOT NULL,
    [user_verified]     BIT            DEFAULT ((0)) NOT NULL,
    [isGroupTicket]     BIT            DEFAULT ((0)) NOT NULL,
    [nameOfGroup]       NVARCHAR (MAX) NULL,
    [numOfGroup]        INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Booking_Ticket] PRIMARY KEY CLUSTERED ([ticket_id] ASC),
    CONSTRAINT [FK_Booking_Ticket_Booking_booking_id] FOREIGN KEY ([booking_id]) REFERENCES [dbo].[Booking] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_Ticket_booking_id]
    ON [dbo].[Booking_Ticket]([booking_id] ASC);

