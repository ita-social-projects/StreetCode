IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240428073027_FactIndexAdd')
BEGIN
    ALTER TABLE [streetcode].[facts] ADD [Index] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240428073027_FactIndexAdd')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240428073027_FactIndexAdd', N'7.0.13');
END;
GO

