IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230804120930_Change_StreetcodeCategoryContent_Text_to_10000_simb')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[sources].[streetcode_source_link_categories]') AND [c].[name] = N'Text');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [sources].[streetcode_source_link_categories] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [sources].[streetcode_source_link_categories] ALTER COLUMN [Text] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230804120930_Change_StreetcodeCategoryContent_Text_to_10000_simb')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230804120930_Change_StreetcodeCategoryContent_Text_to_10000_simb', N'7.0.13');
END;
GO

