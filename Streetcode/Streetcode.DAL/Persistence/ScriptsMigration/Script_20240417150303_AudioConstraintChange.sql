IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240417150303_AudioConstraintChange')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [FK_streetcodes_audios_AudioId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240417150303_AudioConstraintChange')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD CONSTRAINT [FK_streetcodes_audios_AudioId] FOREIGN KEY ([AudioId]) REFERENCES [media].[audios] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240417150303_AudioConstraintChange')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240417150303_AudioConstraintChange', N'7.0.13');
END;
GO

