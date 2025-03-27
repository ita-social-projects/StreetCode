IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250204150151_AddFavourites')
BEGIN
    CREATE TABLE [streetcode].[favourites] (
        [StreetcodeId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_favourites] PRIMARY KEY ([StreetcodeId], [UserId]),
        CONSTRAINT [FK_favourites_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_favourites_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250204150151_AddFavourites')
BEGIN
    CREATE INDEX [IX_favourites_UserId] ON [streetcode].[favourites] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250204150151_AddFavourites')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250204150151_AddFavourites', N'7.0.13');
END;
GO

