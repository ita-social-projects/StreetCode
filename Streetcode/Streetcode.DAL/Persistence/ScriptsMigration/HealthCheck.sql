BEGIN
    IF EXISTS (SELECT 1 FROM [AspNetUsers] WHERE Email = 'admin@admin.com')
    BEGIN
        SELECT * FROM [AspNetUsers] WHERE Email = 'admin@admin.com';
    END
    ELSE
    BEGIN
        SELECT 1 FROM [AspNetUsers];
    END
END;
GO