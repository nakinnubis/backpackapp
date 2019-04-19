CREATE TABLE [dbo].[User_roles] (
    [user_role_id] INT IDENTITY (1, 1) NOT NULL,
    [activity_id]  INT NOT NULL,
    [role_id]      INT NOT NULL,
    [user_id]      INT NOT NULL,
    CONSTRAINT [PK_User_roles] PRIMARY KEY CLUSTERED ([user_role_id] ASC),
    CONSTRAINT [FK_User_roles_Roles_role_id] FOREIGN KEY ([role_id]) REFERENCES [dbo].[Roles] ([role_id]) ON DELETE CASCADE,
    CONSTRAINT [FK_User_roles_User_user_id] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_User_roles_user_id]
    ON [dbo].[User_roles]([user_id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_roles_role_id]
    ON [dbo].[User_roles]([role_id] ASC);

