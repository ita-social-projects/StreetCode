-- IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250328194000_AddedImageHashColumnInImageTable')
-- BEGIN
--     ALTER TABLE [streetcode].[favourites] DROP CONSTRAINT [FK_favourites_streetcodes_StreetcodeId];
-- END;
-- GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250328194000_AddedImageHashColumnInImageTable')
BEGIN
    ALTER TABLE [media].[images] ADD [ImageHash] decimal(20,0) NOT NULL DEFAULT 0.0;
END;
GO

-- IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250328194000_AddedImageHashColumnInImageTable')
-- BEGIN
--     ALTER TABLE [streetcode].[favourites] ADD CONSTRAINT [FK_favourites_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE;
-- END;
-- GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250328194000_AddedImageHashColumnInImageTable')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250328194000_AddedImageHashColumnInImageTable', N'7.0.13');
END;
GO

