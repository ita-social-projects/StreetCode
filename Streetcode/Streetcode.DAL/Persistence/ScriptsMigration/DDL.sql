IF OBJECT_ID(N'[entity_framework].[__EFMigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'entity_framework') IS NULL EXEC(N'CREATE SCHEMA [entity_framework];');
    CREATE TABLE [entity_framework].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'media') IS NULL EXEC(N'CREATE SCHEMA [media];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'add_content') IS NULL EXEC(N'CREATE SCHEMA [add_content];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'streetcode') IS NULL EXEC(N'CREATE SCHEMA [streetcode];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'timeline') IS NULL EXEC(N'CREATE SCHEMA [timeline];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'news') IS NULL EXEC(N'CREATE SCHEMA [news];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'partners') IS NULL EXEC(N'CREATE SCHEMA [partners];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'team') IS NULL EXEC(N'CREATE SCHEMA [team];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'coordinates') IS NULL EXEC(N'CREATE SCHEMA [coordinates];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'feedback') IS NULL EXEC(N'CREATE SCHEMA [feedback];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'sources') IS NULL EXEC(N'CREATE SCHEMA [sources];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'toponyms') IS NULL EXEC(N'CREATE SCHEMA [toponyms];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'transactions') IS NULL EXEC(N'CREATE SCHEMA [transactions];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    IF SCHEMA_ID(N'Users') IS NULL EXEC(N'CREATE SCHEMA [Users];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [media].[audios] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NULL,
        [BlobName] nvarchar(100) NOT NULL,
        [MimeType] nvarchar(10) NOT NULL,
        CONSTRAINT [PK_audios] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [timeline].[historical_contexts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_historical_contexts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [media].[images] (
        [Id] int NOT NULL IDENTITY,
        [BlobName] nvarchar(100) NOT NULL,
        [MimeType] nvarchar(10) NOT NULL,
        CONSTRAINT [PK_images] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [team].[positions] (
        [Id] int NOT NULL IDENTITY,
        [Position] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_positions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [feedback].[responses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NULL,
        [Email] nvarchar(50) NOT NULL,
        [Description] nvarchar(1000) NULL,
        CONSTRAINT [PK_responses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [add_content].[tags] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_tags] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[terms] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        CONSTRAINT [PK_terms] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [toponyms].[toponyms] (
        [Id] int NOT NULL IDENTITY,
        [Oblast] nvarchar(100) NOT NULL,
        [AdminRegionOld] nvarchar(150) NULL,
        [AdminRegionNew] nvarchar(150) NULL,
        [Gromada] nvarchar(150) NULL,
        [Community] nvarchar(150) NULL,
        [StreetName] nvarchar(150) NOT NULL,
        [StreetType] nvarchar(50) NULL,
        CONSTRAINT [PK_toponyms] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [Users].[Users] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Surname] nvarchar(50) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Login] nvarchar(20) NOT NULL,
        [Password] nvarchar(20) NOT NULL,
        [Role] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcodes] (
        [Id] int NOT NULL IDENTITY,
        [Index] int NOT NULL,
        [Teaser] nvarchar(650) NULL,
        [DateString] nvarchar(50) NOT NULL,
        [Alias] nvarchar(50) NULL,
        [Status] int NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [TransliterationUrl] nvarchar(150) NOT NULL,
        [ViewCount] int NOT NULL DEFAULT 0,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
        [EventStartOrPersonBirthDate] datetime2 NOT NULL,
        [EventEndOrPersonDeathDate] datetime2 NULL,
        [AudioId] int NULL,
        [StreetcodeType] nvarchar(max) NOT NULL,
        [FirstName] nvarchar(50) NULL,
        [Rank] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        CONSTRAINT [PK_streetcodes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_streetcodes_audios_AudioId] FOREIGN KEY ([AudioId]) REFERENCES [media].[audios] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [media].[arts] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(400) NULL,
        [Title] nvarchar(150) NULL,
        [ImageId] int NOT NULL,
        CONSTRAINT [PK_arts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_arts_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [media].[image_details] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NULL,
        [Alt] nvarchar(300) NULL,
        [ImageId] int NOT NULL,
        CONSTRAINT [PK_image_details] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_image_details_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [news].[news] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(150) NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [URL] nvarchar(100) NOT NULL,
        [ImageId] int NULL,
        [CreationDate] datetime2 NOT NULL,
        CONSTRAINT [PK_news] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_news_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [partners].[partners] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(255) NOT NULL,
        [LogoId] int NOT NULL,
        [IsKeyPartner] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsVisibleEverywhere] bit NOT NULL,
        [TargetUrl] nvarchar(255) NOT NULL,
        [UrlTitle] nvarchar(255) NULL,
        [Description] nvarchar(600) NULL,
        CONSTRAINT [PK_partners] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_partners_images_LogoId] FOREIGN KEY ([LogoId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [sources].[source_link_categories] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NOT NULL,
        [ImageId] int NOT NULL,
        CONSTRAINT [PK_source_link_categories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_source_link_categories_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [team].[team_members] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(50) NOT NULL,
        [LastName] nvarchar(50) NULL,
        [Description] nvarchar(150) NOT NULL,
        [IsMain] bit NOT NULL,
        [ImageId] int NOT NULL,
        CONSTRAINT [PK_team_members] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_team_members_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[related_terms] (
        [Id] int NOT NULL IDENTITY,
        [Word] nvarchar(50) NOT NULL,
        [TermId] int NOT NULL,
        CONSTRAINT [PK_related_terms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_related_terms_terms_TermId] FOREIGN KEY ([TermId]) REFERENCES [streetcode].[terms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [add_content].[coordinates] (
        [Id] int NOT NULL IDENTITY,
        [Latitude] decimal(18,4) NOT NULL,
        [Longtitude] decimal(18,4) NOT NULL,
        [CoordinateType] nvarchar(max) NOT NULL,
        [StreetcodeId] int NULL,
        [ToponymId] int NULL,
        CONSTRAINT [PK_coordinates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_coordinates_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_coordinates_toponyms_ToponymId] FOREIGN KEY ([ToponymId]) REFERENCES [toponyms].[toponyms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[facts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NOT NULL,
        [FactContent] nvarchar(600) NOT NULL,
        [ImageId] int NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_facts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_facts_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_facts_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[related_figures] (
        [ObserverId] int NOT NULL,
        [TargetId] int NOT NULL,
        CONSTRAINT [PK_related_figures] PRIMARY KEY ([ObserverId], [TargetId]),
        CONSTRAINT [FK_related_figures_streetcodes_ObserverId] FOREIGN KEY ([ObserverId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_related_figures_streetcodes_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_image] (
        [StreetcodeId] int NOT NULL,
        [ImageId] int NOT NULL,
        CONSTRAINT [PK_streetcode_image] PRIMARY KEY ([ImageId], [StreetcodeId]),
        CONSTRAINT [FK_streetcode_image_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_image_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [add_content].[streetcode_tag_index] (
        [StreetcodeId] int NOT NULL,
        [TagId] int NOT NULL,
        [IsVisible] bit NOT NULL,
        [Index] int NOT NULL,
        CONSTRAINT [PK_streetcode_tag_index] PRIMARY KEY ([StreetcodeId], [TagId]),
        CONSTRAINT [FK_streetcode_tag_index_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_tag_index_tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [add_content].[tags] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_toponym] (
        [StreetcodeId] int NOT NULL,
        [ToponymId] int NOT NULL,
        CONSTRAINT [PK_streetcode_toponym] PRIMARY KEY ([StreetcodeId], [ToponymId]),
        CONSTRAINT [FK_streetcode_toponym_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_toponym_toponyms_ToponymId] FOREIGN KEY ([ToponymId]) REFERENCES [toponyms].[toponyms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [add_content].[subtitles] (
        [Id] int NOT NULL IDENTITY,
        [SubtitleText] nvarchar(500) NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_subtitles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_subtitles_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[texts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(300) NOT NULL,
        [TextContent] nvarchar(max) NOT NULL,
        [AdditionalText] nvarchar(500) NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_texts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_texts_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [timeline].[timeline_items] (
        [Id] int NOT NULL IDENTITY,
        [Date] datetime2 NOT NULL,
        [DateViewPattern] int NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [Description] nvarchar(600) NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_timeline_items] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_timeline_items_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [transactions].[transaction_links] (
        [Id] int NOT NULL IDENTITY,
        [UrlTitle] nvarchar(255) NULL,
        [Url] nvarchar(255) NOT NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_transaction_links] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_transaction_links_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [media].[videos] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NULL,
        [Description] nvarchar(max) NULL,
        [Url] nvarchar(max) NOT NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_videos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_videos_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_art] (
        [StreetcodeId] int NOT NULL,
        [ArtId] int NOT NULL,
        [Index] int NOT NULL DEFAULT 1,
        CONSTRAINT [PK_streetcode_art] PRIMARY KEY ([ArtId], [StreetcodeId]),
        CONSTRAINT [FK_streetcode_art_arts_ArtId] FOREIGN KEY ([ArtId]) REFERENCES [media].[arts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_art_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [partners].[partner_source_links] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        [LogoType] tinyint NOT NULL,
        [TargetUrl] nvarchar(255) NOT NULL,
        [PartnerId] int NOT NULL,
        CONSTRAINT [PK_partner_source_links] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_partner_source_links_partners_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [partners].[partners] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_partners] (
        [StreetcodeId] int NOT NULL,
        [PartnerId] int NOT NULL,
        CONSTRAINT [PK_streetcode_partners] PRIMARY KEY ([PartnerId], [StreetcodeId]),
        CONSTRAINT [FK_streetcode_partners_partners_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [partners].[partners] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_partners_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [sources].[streetcode_source_link_categories] (
        [SourceLinkCategoryId] int NOT NULL,
        [StreetcodeId] int NOT NULL,
        [Text] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_streetcode_source_link_categories] PRIMARY KEY ([SourceLinkCategoryId], [StreetcodeId]),
        CONSTRAINT [FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoryId] FOREIGN KEY ([SourceLinkCategoryId]) REFERENCES [sources].[source_link_categories] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_source_link_categories_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [team].[team_member_links] (
        [Id] int NOT NULL IDENTITY,
        [LogoType] tinyint NOT NULL,
        [TargetUrl] nvarchar(255) NOT NULL,
        [TeamMemberId] int NOT NULL,
        CONSTRAINT [PK_team_member_links] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_team_member_links_team_members_TeamMemberId] FOREIGN KEY ([TeamMemberId]) REFERENCES [team].[team_members] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [team].[team_member_positions] (
        [TeamMemberId] int NOT NULL,
        [PositionsId] int NOT NULL,
        CONSTRAINT [PK_team_member_positions] PRIMARY KEY ([TeamMemberId], [PositionsId]),
        CONSTRAINT [FK_team_member_positions_positions_PositionsId] FOREIGN KEY ([PositionsId]) REFERENCES [team].[positions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_team_member_positions_team_members_TeamMemberId] FOREIGN KEY ([TeamMemberId]) REFERENCES [team].[team_members] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [coordinates].[qr_coordinates] (
        [Id] int NOT NULL IDENTITY,
        [QrId] int NOT NULL,
        [Count] int NOT NULL,
        [Address] nvarchar(150) NOT NULL,
        [StreetcodeId] int NOT NULL,
        [StreetcodeCoordinateId] int NOT NULL,
        CONSTRAINT [PK_qr_coordinates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_qr_coordinates_coordinates_StreetcodeCoordinateId] FOREIGN KEY ([StreetcodeCoordinateId]) REFERENCES [add_content].[coordinates] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_qr_coordinates_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE TABLE [HistoricalContextsTimelines] (
        [HistoricalContextId] int NOT NULL,
        [TimelineId] int NOT NULL,
        CONSTRAINT [PK_HistoricalContextsTimelines] PRIMARY KEY ([TimelineId], [HistoricalContextId]),
        CONSTRAINT [FK_HistoricalContextsTimelines_historical_contexts_HistoricalContextId] FOREIGN KEY ([HistoricalContextId]) REFERENCES [timeline].[historical_contexts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_HistoricalContextsTimelines_timeline_items_TimelineId] FOREIGN KEY ([TimelineId]) REFERENCES [timeline].[timeline_items] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_arts_ImageId] ON [media].[arts] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_coordinates_StreetcodeId] ON [add_content].[coordinates] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_coordinates_ToponymId] ON [add_content].[coordinates] ([ToponymId]) WHERE [ToponymId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_facts_ImageId] ON [streetcode].[facts] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_facts_StreetcodeId] ON [streetcode].[facts] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_HistoricalContextsTimelines_HistoricalContextId] ON [HistoricalContextsTimelines] ([HistoricalContextId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_image_details_ImageId] ON [media].[image_details] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_news_ImageId] ON [news].[news] ([ImageId]) WHERE [ImageId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_news_URL] ON [news].[news] ([URL]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_partner_source_links_PartnerId] ON [partners].[partner_source_links] ([PartnerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_partners_LogoId] ON [partners].[partners] ([LogoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_qr_coordinates_StreetcodeCoordinateId] ON [coordinates].[qr_coordinates] ([StreetcodeCoordinateId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_qr_coordinates_StreetcodeId] ON [coordinates].[qr_coordinates] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_related_figures_TargetId] ON [streetcode].[related_figures] ([TargetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_related_terms_TermId] ON [streetcode].[related_terms] ([TermId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_source_link_categories_ImageId] ON [sources].[source_link_categories] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_art_ArtId_StreetcodeId] ON [streetcode].[streetcode_art] ([ArtId], [StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_art_StreetcodeId] ON [streetcode].[streetcode_art] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_image_StreetcodeId] ON [streetcode].[streetcode_image] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_partners_StreetcodeId] ON [streetcode].[streetcode_partners] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_source_link_categories_StreetcodeId] ON [sources].[streetcode_source_link_categories] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_tag_index_TagId] ON [add_content].[streetcode_tag_index] ([TagId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_toponym_ToponymId] ON [streetcode].[streetcode_toponym] ([ToponymId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_streetcodes_AudioId] ON [streetcode].[streetcodes] ([AudioId]) WHERE [AudioId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_streetcodes_Index] ON [streetcode].[streetcodes] ([Index]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_streetcodes_TransliterationUrl] ON [streetcode].[streetcodes] ([TransliterationUrl]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_subtitles_StreetcodeId] ON [add_content].[subtitles] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_team_member_links_TeamMemberId] ON [team].[team_member_links] ([TeamMemberId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_team_member_positions_PositionsId] ON [team].[team_member_positions] ([PositionsId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_team_members_ImageId] ON [team].[team_members] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_texts_StreetcodeId] ON [streetcode].[texts] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_timeline_items_StreetcodeId] ON [timeline].[timeline_items] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_transaction_links_StreetcodeId] ON [transactions].[transaction_links] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    CREATE INDEX [IX_videos_StreetcodeId] ON [media].[videos] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230622110726_Initial')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230622110726_Initial', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

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
    VALUES (N'20230703154732_UpdatePartnerModel', N'6.0.11');
END;
GO

COMMIT;
GO

