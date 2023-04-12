USE [StreetcodeDb]

INSERT INTO [sources].[source_link_source_link_subcategory]
     VALUES
           (1, 1), (2, 1), (3, 1),
           (4, 1), (5, 1), (6, 1),
           (7, 1),
    
           (9, 2), (10, 2), (11, 2),
    
           (1, 3), (2, 3), (3, 3),
           (4, 3), (5, 3), (6, 3),
           (7, 3), (13, 3), (14, 3),
    
           (1, 4), (2, 4), (3, 4),
           (4, 4), (5, 4), (6, 4),
    
           (8, 5), (9, 5), (10, 5),
    
           (12, 6), (13, 6), (14, 6),
    
           (1, 7), (2, 7), (3, 7),
           (4, 7), (5, 7), (6, 7),
           (7, 7), (8, 7), (9, 7),
           (10, 7), (11, 7), (12, 7),
           (13, 7), (14, 7)
;

INSERT INTO [sources].[streetcode_source_link_categories]
    VALUES
           (1, 1),
           (2, 1),
           (3, 1)
;

INSERT INTO [streetcode].[streetcode_partners]
    VALUES
           (2, 1),
           (3, 1),
           (2, 2),
           (3, 3),
           (2, 4)
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
		   (8, 4),
           (15, 5),
           (16, 6),
           (17, 7)
;

INSERT INTO [add_content].[streetcode_tag_index]
(StreetcodeId, TagId, IsVisible, [Index] )
     VALUES
           (1, 1, 1, 1), (1, 2, 1, 2), (1, 3, 1, 3), (1, 5, 1, 4), (1, 6, 1, 5),

           (2, 1, 1, 1), (2, 4, 1, 2), (2, 3, 1, 3), (2, 5, 1, 4), (2, 6, 1, 5),

		   (3, 3, 1, 1), (3, 6, 1, 2),

		   (4, 4, 1, 1),

           (5, 1, 1, 1), (5, 2, 1, 2), (5, 5, 1, 3), (5, 6, 1, 4), (5, 7, 1, 5),

           (6, 1, 1, 1), (6, 3, 1, 2),

           (7, 1, 1, 1), (7, 2, 1, 2), (7, 3, 1, 3), (7, 5, 1, 4), (7, 6, 1, 5)
;

INSERT INTO [streetcode].[streetcode_timeline_item]
     VALUES
           (1, 1), (1, 2), (1, 3), (1, 4), (1, 5),
		   (1, 6), (1, 7), (1, 8), (1, 9), (1, 10)
;

INSERT INTO [timeline].[timeline_item_historical_context]
     VALUES
           (1, 1),
		   (3, 1),
		   (2, 2),
		   (3, 2)
;