IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230905201616_Jobs')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[team].[team_members]') AND [c].[name] = N'LastName');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [team].[team_members] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [team].[team_members] DROP COLUMN [LastName];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230905201616_Jobs')
BEGIN
    IF SCHEMA_ID(N'jobs') IS NULL EXEC(N'CREATE SCHEMA [jobs];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230905201616_Jobs')
BEGIN
    EXEC sp_rename N'[team].[team_members].[FirstName]', N'Name', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230905201616_Jobs')
BEGIN
    CREATE TABLE [jobs].[job] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(65) NOT NULL,
        [Status] bit NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [Salary] nvarchar(15) NOT NULL,
        CONSTRAINT [PK_job] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230905201616_Jobs')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230905201616_Jobs', N'7.0.13');
END;
GO

