CREATE TABLE [dbo].[Avaliability_Pricings] (
    [id]                   INT IDENTITY (1, 1) NOT NULL,
    [avaliabilityId]       INT NOT NULL,
    [individualCategoryId] INT NOT NULL,
    [price]                INT NOT NULL,
    [priceAfterDiscount]   INT NOT NULL,
    CONSTRAINT [PK_Avaliability_Pricings] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Avaliability_Pricings_Avaliability_avaliabilityId] FOREIGN KEY ([avaliabilityId]) REFERENCES [dbo].[Avaliability] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Avaliability_Pricings_Individual_Categories_individualCategoryId] FOREIGN KEY ([individualCategoryId]) REFERENCES [dbo].[Individual_Categories] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Avaliability_Pricings_individualCategoryId]
    ON [dbo].[Avaliability_Pricings]([individualCategoryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Avaliability_Pricings_avaliabilityId]
    ON [dbo].[Avaliability_Pricings]([avaliabilityId] ASC);

