BEGIN
    CREATE TABLE TestTable (
        Id INT PRIMARY KEY,
        Name NVARCHAR(100)
    );
END;
GO

BEGIN
    DROP TABLE TestTable;
END;
GO