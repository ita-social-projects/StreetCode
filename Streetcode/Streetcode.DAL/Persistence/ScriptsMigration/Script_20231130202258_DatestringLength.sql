IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231130202258_DatestringLength')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcodes]') AND [c].[name] = N'DateString');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [streetcode].[streetcodes] ALTER COLUMN [DateString] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231130202258_DatestringLength')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231130202258_DatestringLength', N'7.0.13');
END;
GO

