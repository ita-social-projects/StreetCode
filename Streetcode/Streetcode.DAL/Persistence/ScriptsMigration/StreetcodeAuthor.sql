IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN
    ALTER TABLE [news].[news] DROP CONSTRAINT [FK_news_images_ImageId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD [UserId] nvarchar(450) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN
    CREATE INDEX [IX_streetcodes_UserId] ON [streetcode].[streetcodes] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN
    ALTER TABLE [news].[news] ADD CONSTRAINT [FK_news_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD CONSTRAINT [FK_streetcodes_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN

                    DECLARE @FirstUserId NVARCHAR(450);
                    SELECT TOP 1 @FirstUserId = Id FROM [dbo].[AspNetUsers];

                    UPDATE [streetcode].[streetcodes]
                    SET [UserId] = @FirstUserId
                    WHERE [UserId] IS NULL;
                
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181308_StreetcodeUserRelationship')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250119181308_StreetcodeUserRelationship', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181832_RemoveNullability')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [FK_streetcodes_AspNetUsers_UserId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181832_RemoveNullability')
BEGIN
    DROP INDEX [IX_streetcodes_UserId] ON [streetcode].[streetcodes];
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcodes]') AND [c].[name] = N'UserId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [' + @var0 + '];');
    EXEC(N'UPDATE [streetcode].[streetcodes] SET [UserId] = N'''' WHERE [UserId] IS NULL');
    ALTER TABLE [streetcode].[streetcodes] ALTER COLUMN [UserId] nvarchar(450) NOT NULL;
    ALTER TABLE [streetcode].[streetcodes] ADD DEFAULT N'' FOR [UserId];
    CREATE INDEX [IX_streetcodes_UserId] ON [streetcode].[streetcodes] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181832_RemoveNullability')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD CONSTRAINT [FK_streetcodes_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250119181832_RemoveNullability')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250119181832_RemoveNullability', N'7.0.13');
END;
GO

