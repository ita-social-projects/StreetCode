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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230804120930_Change_StreetcodeCategoryContent_Text_to_10000_simb')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[sources].[streetcode_source_link_categories]') AND [c].[name] = N'Text');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [sources].[streetcode_source_link_categories] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [sources].[streetcode_source_link_categories] ALTER COLUMN [Text] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230804120930_Change_StreetcodeCategoryContent_Text_to_10000_simb')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230804120930_Change_StreetcodeCategoryContent_Text_to_10000_simb', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230905201616_Jobs')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[team].[team_members]') AND [c].[name] = N'LastName');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [team].[team_members] DROP CONSTRAINT [' + @var3 + '];');
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] DROP CONSTRAINT [FK_streetcode_art_arts_ArtId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] DROP CONSTRAINT [FK_streetcode_art_streetcodes_StreetcodeId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] DROP CONSTRAINT [PK_streetcode_art];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    DROP INDEX [IX_streetcode_art_ArtId_StreetcodeId] ON [streetcode].[streetcode_art];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcode_art]') AND [c].[name] = N'StreetcodeId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcode_art] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [streetcode].[streetcode_art] ALTER COLUMN [StreetcodeId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] ADD [Id] int NOT NULL IDENTITY;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] ADD [StreetcodeArtSlideId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [media].[arts] ADD [StreetcodeId] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] ADD CONSTRAINT [PK_streetcode_art] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    CREATE TABLE [streetcode].[streetcode_art_slide] (
        [Id] int NOT NULL IDENTITY,
        [StreetcodeId] int NOT NULL,
        [Template] int NOT NULL,
        [Index] int NOT NULL DEFAULT 1,
        CONSTRAINT [PK_streetcode_art_slide] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_streetcode_art_slide_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    CREATE INDEX [IX_streetcode_art_ArtId_StreetcodeArtSlideId] ON [streetcode].[streetcode_art] ([ArtId], [StreetcodeArtSlideId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    CREATE INDEX [IX_streetcode_art_StreetcodeArtSlideId] ON [streetcode].[streetcode_art] ([StreetcodeArtSlideId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    CREATE INDEX [IX_arts_StreetcodeId] ON [media].[arts] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    CREATE INDEX [IX_streetcode_art_slide_StreetcodeId] ON [streetcode].[streetcode_art_slide] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [media].[arts] ADD CONSTRAINT [FK_arts_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] ADD CONSTRAINT [FK_streetcode_art_arts_ArtId] FOREIGN KEY ([ArtId]) REFERENCES [media].[arts] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] ADD CONSTRAINT [FK_streetcode_art_streetcode_art_slide_StreetcodeArtSlideId] FOREIGN KEY ([StreetcodeArtSlideId]) REFERENCES [streetcode].[streetcode_art_slide] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    ALTER TABLE [streetcode].[streetcode_art] ADD CONSTRAINT [FK_streetcode_art_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231108151054_RefactorStreetcodeArts')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231108151054_RefactorStreetcodeArts', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231130202258_DatestringLength')
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcodes]') AND [c].[name] = N'DateString');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [streetcode].[streetcodes] ALTER COLUMN [DateString] nvarchar(100) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20231130202258_DatestringLength')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231130202258_DatestringLength', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104113247_JobsDescriptionLength')
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[jobs].[job]') AND [c].[name] = N'Description');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [jobs].[job] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [jobs].[job] ALTER COLUMN [Description] nvarchar(3000) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104113247_JobsDescriptionLength')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240104113247_JobsDescriptionLength', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [Users].[Users] DROP CONSTRAINT [PK_Users];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users].[Users]') AND [c].[name] = N'Login');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Users].[Users] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Users].[Users] DROP COLUMN [Login];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users].[Users]') AND [c].[name] = N'Password');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Users].[Users] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [Users].[Users] DROP COLUMN [Password];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users].[Users]') AND [c].[name] = N'Id');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Users].[Users] DROP CONSTRAINT [' + @var9 + '];');
    ALTER TABLE [Users].[Users] DROP COLUMN [Id];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    EXEC sp_rename N'[Users].[Users]', N'AspNetUsers';
    DECLARE @defaultSchema sysname = SCHEMA_NAME();
    EXEC(N'ALTER SCHEMA [' + @defaultSchema + N'] TRANSFER [Users].[AspNetUsers];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    EXEC sp_rename N'[AspNetUsers].[Role]', N'AccessFailedCount', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Email');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var10 + '];');
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Email] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [Id] nvarchar(450) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ConcurrencyStamp] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [EmailConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [LockoutEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [LockoutEnd] datetimeoffset NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [NormalizedEmail] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [NormalizedUserName] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [PasswordHash] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [PhoneNumber] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [PhoneNumberConfirmed] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [SecurityStamp] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [TwoFactorEnabled] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [UserName] nvarchar(256) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240104204923_AddIdentityTables')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240104204923_AddIdentityTables', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240122161255_ForFansLength')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240122161255_ForFansLength', N'7.0.13');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240407205620_AddRefreshTokenToUser')
BEGIN
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[jobs].[job]') AND [c].[name] = N'Description');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [jobs].[job] DROP CONSTRAINT [' + @var11 + '];');
    ALTER TABLE [jobs].[job] ALTER COLUMN [Description] nvarchar(3000) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240407205620_AddRefreshTokenToUser')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [RefreshToken] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240407205620_AddRefreshTokenToUser')
BEGIN
    ALTER TABLE [AspNetUsers] ADD [RefreshTokenExpiry] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240407205620_AddRefreshTokenToUser')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240407205620_AddRefreshTokenToUser', N'7.0.13');
END;
GO

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
    DECLARE @var12 sysname;
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[sources].[source_link_categories]') AND [c].[name] = N'Title');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [sources].[source_link_categories] DROP CONSTRAINT [' + @var12 + '];');
    ALTER TABLE [sources].[source_link_categories] ALTER COLUMN [Title] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    DECLARE @var13 sysname;
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[news].[news]') AND [c].[name] = N'ImageId');
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [news].[news] DROP CONSTRAINT [' + @var13 + '];');
    EXEC(N'UPDATE [news].[news] SET [ImageId] = 0 WHERE [ImageId] IS NULL');
    ALTER TABLE [news].[news] ALTER COLUMN [ImageId] int NOT NULL;
    ALTER TABLE [news].[news] ADD DEFAULT 0 FOR [ImageId];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20240806144219_ImageConstaintChange')
BEGIN
    DECLARE @var14 sysname;
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[jobs].[job]') AND [c].[name] = N'Description');
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [jobs].[job] DROP CONSTRAINT [' + @var14 + '];');
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
    DECLARE @var15 sysname;
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcodes]') AND [c].[name] = N'Teaser');
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [' + @var15 + '];');
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
    DECLARE @var16 sysname;
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[news].[news]') AND [c].[name] = N'URL');
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [news].[news] DROP CONSTRAINT [' + @var16 + '];');
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

