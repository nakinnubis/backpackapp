CREATE TABLE [dbo].[Activity] (
    [id]                             INT             IDENTITY (1, 1) NOT NULL,
    [Activity_length]                DECIMAL (18, 2) NULL,
    [activity_Location]              NVARCHAR (MAX)  NULL,
    [apply_discount]                 BIT             NOT NULL,
    [booking_window]                 INT             NULL,
    [description]                    NVARCHAR (MAX)  NULL,
    [group_price]                    DECIMAL (18, 2) NOT NULL,
    [max_capacity_group]             INT             NULL,
    [meeting_Location]               NVARCHAR (MAX)  NULL,
    [min_capacity_group]             INT             NULL,
    [notice_in_advance]              INT             NULL,
    [rate]                           DECIMAL (18, 2) NULL,
    [remaining_tickets]              INT             NULL,
    [requirements]                   NVARCHAR (MAX)  NULL,
    [status]                         BIT             NOT NULL,
    [title]                          NVARCHAR (MAX)  NULL,
    [totalCapacity]                  INT             NULL,
    [type_id]                        INT             NULL,
    [user_id]                        INT             NULL,
    [bookingAvailableForGroups]      BIT             DEFAULT ((0)) NOT NULL,
    [bookingAvailableForIndividuals] BIT             DEFAULT ((0)) NOT NULL,
    [isCompleted]                    BIT             DEFAULT ((0)) NOT NULL,
    [stepNumber]                     INT             DEFAULT ((0)) NOT NULL,
    [LocationFlag]                   BIT             DEFAULT ((0)) NOT NULL,
    [activity_Lang]                  DECIMAL (18, 2) NULL,
    [activity_Lat]                   DECIMAL (18, 2) NULL,
    [meeting_Lang]                   DECIMAL (18, 2) NULL,
    [meeting_Lat]                    DECIMAL (18, 2) NULL,
    [isdeleted]                      BIT             DEFAULT ((0)) NOT NULL,
    [capacityIsUnlimited]            BIT             DEFAULT ((0)) NOT NULL,
    [price_discount]                 REAL            NOT NULL,
    [has_specific_capacity] BIT NULL DEFAULT ((0)), 
    [has_individual_categories] BIT NULL DEFAULT ((0)), 
    [individual_categories] NVARCHAR(MAX) NULL, 
    [modified_date] DATE NULL, 
    [activity_option] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Activity_ActivityType_type_id] FOREIGN KEY ([type_id]) REFERENCES [dbo].[ActivityType] ([id]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_Activity_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id])	 

);
 

GO
CREATE NONCLUSTERED INDEX [IX_Activity_user_id]
    ON [dbo].[Activity]([user_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Activity_type_id]
    ON [dbo].[Activity]([type_id] ASC);

