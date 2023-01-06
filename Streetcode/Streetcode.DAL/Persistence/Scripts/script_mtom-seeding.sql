USE [StreetcodeDb]

INSERT INTO [sources].[source_link_source_link_subcategory]
     VALUES
           (1, 3),
		   (2, 1),
		   (2, 3),
		   (3, 3)
;

INSERT INTO [streetcode].[streetcode_arts]
     VALUES
           (1, 1),
		   (2, 1),
		   (3, 1)
;

INSERT INTO [streetcode].[streetcode_fact]
     VALUES
           (1, 1),
		   (1, 2),
		   (2, 2)
;

INSERT INTO [streetcode].[streetcode_image]
     VALUES
           (1, 1),
		   (6, 2),
		   (7, 3),
		   (8, 4)
;

INSERT INTO [streetcode].[streetcode_tag]
     VALUES
           (1, 1),
		   (1, 2),
		   (2, 4),
		   (2, 3),
		   (3, 3),
		   (4, 4)
;

INSERT INTO [streetcode].[streetcode_timeline_item]
     VALUES
           (1, 1),
		   (2, 1),
		   (1, 2)
;

INSERT INTO [streetcode].[streetcode_toponym]
     VALUES
           (1, 1),
		   (2, 1),
		   (2, 3)
;

INSERT INTO [timeline].[timeline_item_historical_context]
     VALUES
           (1, 1),
		   (3, 1),
		   (2, 2),
		   (3, 2)
;