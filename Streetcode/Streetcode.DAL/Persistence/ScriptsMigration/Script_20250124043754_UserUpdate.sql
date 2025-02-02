IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    ALTER TABLE [news].[news] DROP CONSTRAINT [FK_news_images_ImageId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    IF SCHEMA_ID(N'users') IS NULL EXEC(N'CREATE SCHEMA [users];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'UserName');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var0 + '];');
    EXEC(N'UPDATE [AspNetUsers] SET [UserName] = N'''' WHERE [UserName] IS NULL');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [UserName] nvarchar(256) NOT NULL;
    ALTER TABLE [AspNetUsers] ADD DEFAULT N'' FOR [UserName];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Surname');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Surname] nvarchar(128) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Name');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Name] nvarchar(128) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [AboutYourself] nvarchar(500) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [AvatarId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    CREATE TABLE [users].[expertise] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_expertise] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    CREATE TABLE [users].[user_expertise] (
        [ExpertiseId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_user_expertise] PRIMARY KEY ([ExpertiseId], [UserId]),
        CONSTRAINT [FK_user_expertise_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_user_expertise_expertise_ExpertiseId] FOREIGN KEY ([ExpertiseId]) REFERENCES [users].[expertise] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_AspNetUsers_AvatarId] ON [AspNetUsers] ([AvatarId]) WHERE [AvatarId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    CREATE INDEX [IX_user_expertise_UserId] ON [users].[user_expertise] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_images_AvatarId] FOREIGN KEY ([AvatarId]) REFERENCES [media].[images] ([Id]) ON DELETE SET NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    ALTER TABLE [news].[news] ADD CONSTRAINT [FK_news_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250124043754_UserUpdate')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250124043754_UserUpdate', N'7.0.13');
END;
GO

