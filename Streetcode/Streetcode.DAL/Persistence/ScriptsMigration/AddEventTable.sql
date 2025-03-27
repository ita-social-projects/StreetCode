IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    IF SCHEMA_ID(N'events') IS NULL EXEC(N'CREATE SCHEMA [events];');
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    CREATE TABLE [events].[event] (
        [Id] int NOT NULL IDENTITY,
        [Date] datetime2 NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NULL,
        [EventType] nvarchar(max) NOT NULL,
        [Location] nvarchar(200) NULL,
        [Organizer] nvarchar(200) NULL,
        [DateString] nvarchar(100) NULL,
        [TimelineItemId] int NULL,
        CONSTRAINT [PK_event] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_event_timeline_items_TimelineItemId] FOREIGN KEY ([TimelineItemId]) REFERENCES [timeline].[timeline_items] ([Id]) ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    CREATE TABLE [EventStreetcodes] (
        [EventId] int NOT NULL,
        [StreetcodeId] int NOT NULL,
        CONSTRAINT [PK_EventStreetcodes] PRIMARY KEY ([EventId], [StreetcodeId]),
        CONSTRAINT [FK_EventStreetcodes_event_EventId] FOREIGN KEY ([EventId]) REFERENCES [events].[event] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_EventStreetcodes_streetcodes_StreetcodeId] FOREIGN KEY ([StreetcodeId]) REFERENCES [streetcode].[streetcodes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    CREATE INDEX [IX_event_TimelineItemId] ON [events].[event] ([TimelineItemId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    CREATE INDEX [IX_EventStreetcodes_StreetcodeId] ON [EventStreetcodes] ([StreetcodeId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    ALTER TABLE [news].[news] ADD CONSTRAINT [FK_news_images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [media].[images] ([Id]);
END;
GO

IF NOT EXISTS(SELECT * FROM [entity_framework].[__EFMigrationsHistory] WHERE [MigrationId] = N'20250213203537_AddEventTable')
BEGIN
    INSERT INTO [entity_framework].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250213203537_AddEventTable', N'7.0.13');
END;
GO

