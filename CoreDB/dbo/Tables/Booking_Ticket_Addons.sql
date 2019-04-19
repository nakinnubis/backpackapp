CREATE TABLE [dbo].[Booking_Ticket_Addons] (
    [booking_ticket_addons_id] INT IDENTITY (1, 1) NOT NULL,
    [addon_id]                 INT NOT NULL,
    [ticket_id]                INT NOT NULL,
    [addonCount]               INT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Booking_Ticket_Addons] PRIMARY KEY CLUSTERED ([booking_ticket_addons_id] ASC),
    CONSTRAINT [FK_Booking_Ticket_Addons_Activity_Add_Ons_addon_id] FOREIGN KEY ([addon_id]) REFERENCES [dbo].[Activity_Add_Ons] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Booking_Ticket_Addons_Booking_Ticket_ticket_id] FOREIGN KEY ([ticket_id]) REFERENCES [dbo].[Booking_Ticket] ([ticket_id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_Ticket_Addons_addon_id]
    ON [dbo].[Booking_Ticket_Addons]([addon_id] ASC);

