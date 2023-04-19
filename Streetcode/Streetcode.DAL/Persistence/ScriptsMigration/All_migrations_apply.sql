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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'media') IS NULL EXEC(N'CREATE SCHEMA [media];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'add_content') IS NULL EXEC(N'CREATE SCHEMA [add_content];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'feedback') IS NULL EXEC(N'CREATE SCHEMA [feedback];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'streetcode') IS NULL EXEC(N'CREATE SCHEMA [streetcode];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'timeline') IS NULL EXEC(N'CREATE SCHEMA [timeline];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'partners') IS NULL EXEC(N'CREATE SCHEMA [partners];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'sources') IS NULL EXEC(N'CREATE SCHEMA [sources];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'toponyms') IS NULL EXEC(N'CREATE SCHEMA [toponyms];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    IF SCHEMA_ID(N'transactions') IS NULL EXEC(N'CREATE SCHEMA [transactions];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [feedback].[donations] (
        [Id] int NOT NULL IDENTITY,
        CONSTRAINT [PK_donations] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [timeline].[historical_contexts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_historical_contexts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [media].[images] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NULL,
        [Alt] nvarchar(100) NULL,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_images] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [feedback].[responses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NULL,
        [Email] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_responses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [sources].[source_links] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(300) NULL,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_source_links] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcodes] (
        [Id] int NOT NULL IDENTITY,
        [Index] int NOT NULL,
        [Teaser] nvarchar(max) NOT NULL,
        [ViewCount] int NOT NULL DEFAULT 0,
        [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
        [UpdatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
        [EventStartOrPersonBirthDate] datetime2 NOT NULL,
        [EventEndOrPersonDeathDate] datetime2 NOT NULL,
        [StreetcodeType] nvarchar(max) NOT NULL,
        [Title] nvarchar(100) NULL,
        [FirstName] nvarchar(50) NULL,
        [Rank] nvarchar(50) NULL,
        [LastName] nvarchar(50) NULL,
        CONSTRAINT [PK_streetcodes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [add_content].[tags] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_tags] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[terms] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_terms] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [timeline].[timeline_items] (
        [Id] int NOT NULL IDENTITY,
        [Date] datetime2 NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_timeline_items] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [media].[arts] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(max) NULL,
        [ImageId] int NOT NULL,
        CONSTRAINT [PK_arts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_arts_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[facts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NOT NULL,
        [FactContent] nvarchar(max) NOT NULL,
        [ImageId] int NULL,
        CONSTRAINT [PK_facts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_facts_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [partners].[partners] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [LogoId] int NOT NULL,
        [IsKeyPartner] bit NOT NULL DEFAULT CAST(0 AS bit),
        [TargetUrl] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_partners] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_partners_images_LogoId] FOREIGN KEY ([LogoId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [media].[audios] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NULL,
        [Description] nvarchar(max) NULL,
        [Url] nvarchar(max) NOT NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_audios] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_audios_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_image] (
        [ImagesId] int NOT NULL,
        [StreetcodesId] int NOT NULL,
        CONSTRAINT [PK_streetcode_image] PRIMARY KEY ([ImagesId], [StreetcodesId]),
        CONSTRAINT [FK_streetcode_image_images_ImagesId] FOREIGN KEY ([ImagesId]) REFERENCES [media].[images] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_image_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [add_content].[subtitles] (
        [Id] int NOT NULL IDENTITY,
        [Status] tinyint NOT NULL,
        [FirstName] nvarchar(50) NOT NULL,
        [LastName] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Title] nvarchar(50) NULL,
        [Url] nvarchar(max) NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_subtitles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_subtitles_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[texts] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NOT NULL,
        [TextContent] nvarchar(max) NOT NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_texts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_texts_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [transactions].[transaction_links] (
        [Id] int NOT NULL IDENTITY,
        [UrlTitle] nvarchar(max) NULL,
        [Url] nvarchar(max) NOT NULL,
        [QrCodeUrl] nvarchar(max) NOT NULL,
        [QrCodeUrlTitle] nvarchar(max) NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_transaction_links] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_transaction_links_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_tag] (
        [StreetcodesId] int NOT NULL,
        [TagsId] int NOT NULL,
        CONSTRAINT [PK_streetcode_tag] PRIMARY KEY ([StreetcodesId], [TagsId]),
        CONSTRAINT [FK_streetcode_tag_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_tag_tags_TagsId] FOREIGN KEY ([TagsId]) REFERENCES [add_content].[tags] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_timeline_item] (
        [StreetcodesId] int NOT NULL,
        [TimelineItemsId] int NOT NULL,
        CONSTRAINT [PK_streetcode_timeline_item] PRIMARY KEY ([StreetcodesId], [TimelineItemsId]),
        CONSTRAINT [FK_streetcode_timeline_item_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_timeline_item_timeline_items_TimelineItemsId] FOREIGN KEY ([TimelineItemsId]) REFERENCES [timeline].[timeline_items] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [timeline].[timeline_item_historical_context] (
        [HistoricalContextsId] int NOT NULL,
        [TimelineItemsId] int NOT NULL,
        CONSTRAINT [PK_timeline_item_historical_context] PRIMARY KEY ([HistoricalContextsId], [TimelineItemsId]),
        CONSTRAINT [FK_timeline_item_historical_context_historical_contexts_HistoricalContextsId] FOREIGN KEY ([HistoricalContextsId]) REFERENCES [timeline].[historical_contexts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_timeline_item_historical_context_timeline_items_TimelineItemsId] FOREIGN KEY ([TimelineItemsId]) REFERENCES [timeline].[timeline_items] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_toponym] (
        [StreetcodesId] int NOT NULL,
        [ToponymsId] int NOT NULL,
        CONSTRAINT [PK_streetcode_toponym] PRIMARY KEY ([StreetcodesId], [ToponymsId]),
        CONSTRAINT [FK_streetcode_toponym_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_toponym_toponyms_ToponymsId] FOREIGN KEY ([ToponymsId]) REFERENCES [toponyms].[toponyms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
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

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_fact] (
        [FactsId] int NOT NULL,
        [StreetcodesId] int NOT NULL,
        CONSTRAINT [PK_streetcode_fact] PRIMARY KEY ([FactsId], [StreetcodesId]),
        CONSTRAINT [FK_streetcode_fact_facts_FactsId] FOREIGN KEY ([FactsId]) REFERENCES [streetcode].[facts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_fact_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [partners].[partner_source_links] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(50) NOT NULL,
        [LogoUrl] nvarchar(max) NOT NULL,
        [TargetUrl] nvarchar(max) NOT NULL,
        [PartnerId] int NOT NULL,
        CONSTRAINT [PK_partner_source_links] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_partner_source_links_partners_PartnerId] FOREIGN KEY ([PartnerId]) REFERENCES [partners].[partners] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [streetcode].[streetcode_partners] (
        [PartnersId] int NOT NULL,
        [StreetcodesId] int NOT NULL,
        CONSTRAINT [PK_streetcode_partners] PRIMARY KEY ([PartnersId], [StreetcodesId]),
        CONSTRAINT [FK_streetcode_partners_partners_PartnersId] FOREIGN KEY ([PartnersId]) REFERENCES [partners].[partners] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_partners_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [sources].[source_link_subcategories] (
        [Id] int NOT NULL IDENTITY,
        [Title] nvarchar(100) NOT NULL,
        [SourceLinkCategoryId] int NOT NULL,
        CONSTRAINT [PK_source_link_subcategories] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_source_link_subcategories_source_link_categories_SourceLinkCategoryId] FOREIGN KEY ([SourceLinkCategoryId]) REFERENCES [sources].[source_link_categories] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [sources].[streetcode_source_link_categories] (
        [SourceLinkCategoriesId] int NOT NULL,
        [StreetcodesId] int NOT NULL,
        CONSTRAINT [PK_streetcode_source_link_categories] PRIMARY KEY ([SourceLinkCategoriesId], [StreetcodesId]),
        CONSTRAINT [FK_streetcode_source_link_categories_source_link_categories_SourceLinkCategoriesId] FOREIGN KEY ([SourceLinkCategoriesId]) REFERENCES [sources].[source_link_categories] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_streetcode_source_link_categories_streetcodes_StreetcodesId] FOREIGN KEY ([StreetcodesId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE TABLE [sources].[source_link_source_link_subcategory] (
        [SourceLinksId] int NOT NULL,
        [SubCategoriesId] int NOT NULL,
        CONSTRAINT [PK_source_link_source_link_subcategory] PRIMARY KEY ([SourceLinksId], [SubCategoriesId]),
        CONSTRAINT [FK_source_link_source_link_subcategory_source_link_subcategories_SubCategoriesId] FOREIGN KEY ([SubCategoriesId]) REFERENCES [sources].[source_link_subcategories] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_source_link_source_link_subcategory_source_links_SourceLinksId] FOREIGN KEY ([SourceLinksId]) REFERENCES [sources].[source_links] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_arts_ImageId] ON [media].[arts] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_audios_StreetcodeId] ON [media].[audios] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_coordinates_StreetcodeId] ON [add_content].[coordinates] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_coordinates_ToponymId] ON [add_content].[coordinates] ([ToponymId]) WHERE [ToponymId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_facts_ImageId] ON [streetcode].[facts] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_partner_source_links_PartnerId] ON [partners].[partner_source_links] ([PartnerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_partners_LogoId] ON [partners].[partners] ([LogoId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_related_figures_TargetId] ON [streetcode].[related_figures] ([TargetId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_source_link_categories_ImageId] ON [sources].[source_link_categories] ([ImageId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_source_link_source_link_subcategory_SubCategoriesId] ON [sources].[source_link_source_link_subcategory] ([SubCategoriesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_source_link_subcategories_SourceLinkCategoryId] ON [sources].[source_link_subcategories] ([SourceLinkCategoryId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_art_ArtId_StreetcodeId] ON [streetcode].[streetcode_art] ([ArtId], [StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_art_StreetcodeId] ON [streetcode].[streetcode_art] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_fact_StreetcodesId] ON [streetcode].[streetcode_fact] ([StreetcodesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_image_StreetcodesId] ON [streetcode].[streetcode_image] ([StreetcodesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_partners_StreetcodesId] ON [streetcode].[streetcode_partners] ([StreetcodesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_source_link_categories_StreetcodesId] ON [sources].[streetcode_source_link_categories] ([StreetcodesId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_tag_TagsId] ON [streetcode].[streetcode_tag] ([TagsId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_timeline_item_TimelineItemsId] ON [streetcode].[streetcode_timeline_item] ([TimelineItemsId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_streetcode_toponym_ToponymsId] ON [streetcode].[streetcode_toponym] ([ToponymsId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_subtitles_StreetcodeId] ON [add_content].[subtitles] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_texts_StreetcodeId] ON [streetcode].[texts] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_timeline_item_historical_context_TimelineItemsId] ON [timeline].[timeline_item_historical_context] ([TimelineItemsId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE UNIQUE INDEX [IX_transaction_links_StreetcodeId] ON [transactions].[transaction_links] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    CREATE INDEX [IX_videos_StreetcodeId] ON [media].[videos] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129185745_Initial')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230129185745_Initial', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230129191410_RemoveToponymSeeding')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230129191410_RemoveToponymSeeding', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230130173038_ChangeArts')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230130173038_ChangeArts', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230201154252_Added types of icons for PartnerLink model')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[partners].[partner_source_links]') AND [c].[name] = N'LogoUrl');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [partners].[partner_source_links] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [partners].[partner_source_links] DROP COLUMN [LogoUrl];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230201154252_Added types of icons for PartnerLink model')
BEGIN
    ALTER TABLE [partners].[partners] ADD [UrlTitle] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230201154252_Added types of icons for PartnerLink model')
BEGIN
    ALTER TABLE [partners].[partner_source_links] ADD [LogoType] tinyint NOT NULL DEFAULT CAST(0 AS tinyint);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230201154252_Added types of icons for PartnerLink model')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230201154252_Added types of icons for PartnerLink model', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230303172636_AddAlias_DateString_UpdateTitle_Status')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[streetcodes]') AND [c].[name] = N'Title');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[streetcodes] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [streetcode].[streetcodes] ALTER COLUMN [Title] nvarchar(100) NOT NULL;
    ALTER TABLE [streetcode].[streetcodes] ADD DEFAULT N'' FOR [Title];
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230303172636_AddAlias_DateString_UpdateTitle_Status')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD [Alias] nvarchar(30) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230303172636_AddAlias_DateString_UpdateTitle_Status')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD [DateString] nvarchar(50) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230303172636_AddAlias_DateString_UpdateTitle_Status')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD [Status] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230303172636_AddAlias_DateString_UpdateTitle_Status')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230303172636_AddAlias_DateString_UpdateTitle_Status', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230314083059_AddRelatedWordsToTerms')
BEGIN
    CREATE TABLE [streetcode].[related_terms] (
        [Id] int NOT NULL IDENTITY,
        [Word] nvarchar(max) NOT NULL,
        [TermId] int NOT NULL,
        CONSTRAINT [PK_related_terms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_related_terms_terms_TermId] FOREIGN KEY ([TermId]) REFERENCES [streetcode].[terms] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230314083059_AddRelatedWordsToTerms')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230314083059_AddRelatedWordsToTerms', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230317144844_Change Audio and Image tables')
BEGIN
    EXEC sp_rename N'[media].[images].[Url]', N'MimeType', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230317144844_Change Audio and Image tables')
BEGIN
    EXEC sp_rename N'[media].[audios].[Url]', N'MimeType', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230317144844_Change Audio and Image tables')
BEGIN
    ALTER TABLE [media].[images] ADD [BlobStorageName] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230317144844_Change Audio and Image tables')
BEGIN
    ALTER TABLE [media].[audios] ADD [BlobStorageName] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230317144844_Change Audio and Image tables')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230317144844_Change Audio and Image tables', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230322083631_BlobStorageName to BlobName')
BEGIN
    EXEC sp_rename N'[media].[images].[BlobStorageName]', N'BlobName', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230322083631_BlobStorageName to BlobName')
BEGIN
    EXEC sp_rename N'[media].[audios].[BlobStorageName]', N'BlobName', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230322083631_BlobStorageName to BlobName')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230322083631_BlobStorageName to BlobName', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230323123612_AddTransliterationUrlToStreetcode')
BEGIN
    ALTER TABLE [streetcode].[streetcodes] ADD [TransliterationUrl] nvarchar(150) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230323123612_AddTransliterationUrlToStreetcode')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[streetcode].[related_terms]') AND [c].[name] = N'Word');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [streetcode].[related_terms] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [streetcode].[related_terms] ALTER COLUMN [Word] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230323123612_AddTransliterationUrlToStreetcode')
BEGIN
    CREATE UNIQUE INDEX [IX_streetcodes_TransliterationUrl] ON [streetcode].[streetcodes] ([TransliterationUrl]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230323123612_AddTransliterationUrlToStreetcode')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230323123612_AddTransliterationUrlToStreetcode', N'6.0.11');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20230405101458_Fixes seeding')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230405101458_Fixes seeding', N'6.0.11');
END;
GO

COMMIT;
GO

