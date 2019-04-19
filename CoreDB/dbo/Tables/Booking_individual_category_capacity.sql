CREATE TABLE [dbo].[Booking_individual_category_capacity] (
    [id]          INT IDENTITY (1, 1) NOT NULL,
    [booking_id]  INT NOT NULL,
    [category_id] INT NOT NULL,
    [count]       INT NOT NULL,
    CONSTRAINT [PK_Booking_individual_category_capacity] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Booking_individual_category_capacity_Booking_booking_id] FOREIGN KEY ([booking_id]) REFERENCES [dbo].[Booking] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Booking_individual_category_capacity_Individual_Categories_category_id] FOREIGN KEY ([category_id]) REFERENCES [dbo].[Individual_Categories] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_individual_category_capacity_category_id]
    ON [dbo].[Booking_individual_category_capacity]([category_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Booking_individual_category_capacity_booking_id]
    ON [dbo].[Booking_individual_category_capacity]([booking_id] ASC);

