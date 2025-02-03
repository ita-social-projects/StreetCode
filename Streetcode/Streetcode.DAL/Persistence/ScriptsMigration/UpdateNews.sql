IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250203093100_NewsUpdate')
BEGIN
    ALTER TABLE news.news DROP CONSTRAINT NewsTextMaxLength;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250203093100_NewsUpdate')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250203093100_NewsUpdate', N'7.0.13');
END;
GO

