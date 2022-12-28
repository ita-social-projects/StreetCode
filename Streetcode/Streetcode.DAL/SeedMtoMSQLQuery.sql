USE [StreetcodeDb]
GO

INSERT INTO [sources].[SourceLinkSourceLinkCategory]
           ([SourceLinksId]
		   ,[CategoriesId])
     VALUES
           (1,3),
		   (2,1),
		   (2,3),
		   (3,3);
GO

INSERT INTO [streetcode].[streetcode_arts]
           ([ArtsId]
           ,[StreetcodesId])
     VALUES
           (1,1),
		   (2,1),
		   (3,1)
GO

INSERT INTO [streetcode].[streetcode_fact]
           ([FactsId]
           ,[StreetcodesId])
     VALUES
           (1,1),
		   (1,1)
GO

INSERT INTO [streetcode].[streetcode_image]
           ([ImagesId]
           ,[StreetcodesId])
     VALUES
           (1,1),
		   (6,2),
		   (7,3),
		   (8,4)
GO

INSERT INTO [streetcode].[streetcode_tag]
           ([StreetcodesId]
           ,[TagsId])
     VALUES
           (1,1),
		   (1,2),
		   (2,1),
		   (2,3),
		   (3,3),
		   (4,4)
GO

INSERT INTO [streetcode].[streetcode_timeline_item]
           ([TimelineItemsId]
		   ,[StreetcodesId])
     VALUES
           (1,1),
		   (2,1)
GO

INSERT INTO [streetcode].[streetcode_toponym]
           ([ToponymsId]
		   ,[StreetcodesId])
     VALUES
           (1,1),
		   (2,1),
		   (3,4)
GO

INSERT INTO [timeline].[timeline_item_historical_context]
           ([TimelineItemsId]
		   ,[HistoricalContextsId])
     VALUES
           (1,1),
		   (1,3),
		   (2,2),
		   (2,3)
GO








