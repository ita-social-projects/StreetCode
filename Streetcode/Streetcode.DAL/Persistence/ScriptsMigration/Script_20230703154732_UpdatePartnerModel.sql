IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230703154732_UpdatePartnerModel')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[partners].[partner_source_links]') AND [c].[name] = N'Title');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [partners].[partner_source_links] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [partners].[partner_source_links] DROP COLUMN [Title];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230703154732_UpdatePartnerModel')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[partners].[partners]') AND [c].[name] = N'TargetUrl');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [partners].[partners] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [partners].[partners] ALTER COLUMN [TargetUrl] nvarchar(255) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230703154732_UpdatePartnerModel')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230703154732_UpdatePartnerModel', N'7.0.13');
END;
GO

