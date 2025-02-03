IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    ALTER TABLE [news].[news] DROP CONSTRAINT [FK_news_images_ImageId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    DROP INDEX [IX_news_ImageId] ON [news].[news];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[sources].[source_link_categories]') AND [c].[name] = N'Title');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [sources].[source_link_categories] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [sources].[source_link_categories] ALTER COLUMN [Title] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[news].[news]') AND [c].[name] = N'ImageId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [news].[news] DROP CONSTRAINT [' + @var1 + '];');
    EXEC(N'UPDATE [news].[news] SET [ImageId] = 0 WHERE [ImageId] IS NULL');
    ALTER TABLE [news].[news] ALTER COLUMN [ImageId] int NOT NULL;
    ALTER TABLE [news].[news] ADD DEFAULT 0 FOR [ImageId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[jobs].[job]') AND [c].[name] = N'Description');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [jobs].[job] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [jobs].[job] ALTER COLUMN [Description] nvarchar(3000) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    CREATE UNIQUE INDEX [IX_news_ImageId] ON [news].[news] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    ALTER TABLE [news].[news] ADD CONSTRAINT [FK_news_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240806144219_ImageConstaintChange', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240905125058_MakeTeaserRequired')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcodes]') AND [c].[name] = N'Teaser');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [' + @var3 + '];');
    EXEC(N'UPDATE [streetcode].[streetcodes] SET [Teaser] = N'''' WHERE [Teaser] IS NULL');
    ALTER TABLE [streetcode].[streetcodes] ALTER COLUMN [Teaser] nvarchar(650) NOT NULL;
    ALTER TABLE [streetcode].[streetcodes] ADD DEFAULT N'' FOR [Teaser];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240905125058_MakeTeaserRequired')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240905125058_MakeTeaserRequired', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240906091812_AddConstraintsToNews')
BEGIN
    DROP INDEX [IX_news_URL] ON [news].[news];
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[news].[news]') AND [c].[name] = N'URL');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [news].[news] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [news].[news] ALTER COLUMN [URL] nvarchar(200) NOT NULL;
    CREATE UNIQUE INDEX [IX_news_URL] ON [news].[news] ([URL]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240906091812_AddConstraintsToNews')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240906091812_AddConstraintsToNews', N'7.0.13');
END;
GO

