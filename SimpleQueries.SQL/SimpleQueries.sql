CREATE OR ALTER PROCEDURE [dbo].[sp_Get_Addresses] @City1 [nvarchar](255), @City2 [nvarchar](255), @Street [nvarchar](255)
AS
BEGIN
    SELECT *
    FROM [dbo].[Address] [Address]
	WHERE [Address].[City] = @City1 OR [Address].[City] = @City2
	ORDER BY [Address].[City] DESC;

	SELECT [Address].[AddressId], [Address].[Building]
	FROM [dbo].[Address] [Address]
	WHERE [Address].[City] = @City1 AND [Address].[Street] = @Street
	ORDER BY [Address].[Building] ASC;
END;
GO

EXEC [dbo].[sp_Get_Addresses] @City1 = 'New York', @City2 = 'Tokyo', @Street = 'Broadway';
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Count_Tenants_With_Debt]
AS
BEGIN
    SELECT DISTINCT COUNT([Tenant].[TenantId]) AS [CountTenantsWithDebt]
	FROM [dbo].[Tenant] [Tenant]
	RIGHT JOIN [dbo].[Bill] AS [Bill] ON [Bill].[TenantId] = [Tenant].[TenantId]
	WHERE [Bill].[EndDate] IS NULL;
END;
GO

EXEC [dbo].[sp_Count_Tenants_With_Debt];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Owner_Tenant_Relations] @TenantId [uniqueidentifier], @OwnerId [uniqueidentifier]
AS
BEGIN
    SELECT [Rent].[StartDate], [Rent].[AssetId]
	FROM [dbo].[Tenant] [Tenant]
	RIGHT JOIN [dbo].[Rent] AS [Rent] ON [Rent].[TenantId] = [Tenant].[TenantId]
	LEFT JOIN [dbo].[Asset] AS [Asset] ON [Asset].[AssetId] = [Rent].[AssetId]
	WHERE [Tenant].[TenantId] = @TenantId AND [Asset].[OwnerId] = @OwnerId
	ORDER BY [Rent].[StartDate] ASC;

	SELECT [Address].[City], [Tenant].[TenantId], [Tenant].[Name] AS [TenantName], [Owner].[OwnerId], [Owner].[Name] AS [OwnerName]
	FROM [dbo].[Address] [Address]
	RIGHT JOIN [dbo].[Tenant] AS [Tenant] ON [Tenant].[AddressId] = [Address].[AddressId]
	RIGHT JOIN [dbo].[Owner] AS [Owner] ON [Owner].[AddressId] = [Address].[AddressId]
	WHERE [Tenant].[TenantId] = @TenantId OR [Owner].[OwnerId] = @OwnerId
	ORDER BY [Address].[City];
END;
GO

DECLARE @TenantId [uniqueidentifier], @OwnerId [uniqueidentifier];

SELECT @TenantId = [Tenant].[TenantId]
FROM [dbo].[Tenant] [Tenant]
WHERE [Tenant].[Name] = 'Olivia Brown';

SELECT @OwnerId = [Owner].[OwnerId]
FROM [dbo].[Owner] [Owner]
WHERE [Owner].[Name] = 'Emily Martinez';

EXEC [dbo].[sp_Owner_Tenant_Relations] @TenantId = @TenantId, @OwnerId = @OwnerId;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Owners_With_No_Assets]
AS
BEGIN
    SELECT [Owner].[OwnerId], [Owner].[Name]
	FROM [dbo].[Owner] [owner]
	FULL OUTER JOIN [dbo].[Asset] AS [Asset] ON [Asset].[OwnerId] = [Owner].[OwnerId]
	WHERE [Owner].[OwnerId] NOT IN(
	    SELECT [Owner].[OwnerId]
		FROM [dbo].[Owner]
		INNER JOIN [dbo].[Asset] AS [Asset] ON [Asset].[OwnerId] = [Owner].[OwnerId]
	);
END;
GO

EXEC [dbo].[sp_Owners_With_No_Assets];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Find_Tenants_By_Director_First_Name] @DirectorFirstName [nvarchar](50)
AS
BEGIN
    SELECT [Tenant].[TenantId], [Tenant].[Name], [Tenant].[Director]
	FROM [dbo].[Tenant] [Tenant]
	WHERE [Tenant].[Director] LIKE CONCAT(@DirectorFirstName, '%');
END;
GO

EXEC [dbo].[sp_Find_Tenants_By_Director_First_Name] @DirectorFirstName = 'Jane';
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Bills_Issued_In_Some_Period] @StartPeriod [datetime2], @EndPeriod [datetime2]
AS
BEGIN
    SELECT [Bill].[BillId], [Bill].[BillAmount], [Bill].[IssueDate]
	FROM [dbo].[Bill] [Bill]
	WHERE [Bill].[IssueDate] BETWEEN @StartPeriod AND @EndPeriod;
END;
GO

EXEC [dbo].[sp_Bills_Issued_In_Some_Period] @StartPeriod = '2024-02-13 00:00:00.0000000', @EndPeriod = '2024-02-15 00:00:00.0000000';
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Tenants_With_No_Debt]
AS
BEGIN
    SELECT [Tenant].[TenantId], [Tenant].[Name]
	FROM [dbo].[Tenant] [Tenant]
	WHERE [Tenant].[TenantId] NOT IN(
	    SELECT [Bill].[TenantId]
		FROM [dbo].[Bill] [Bill]
		WHERE [Bill].[EndDate] IS NOT NULL
	);
END;
GO

EXEC [dbo].[sp_Tenants_With_No_Debt];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Room_With_Recorded_Highest_Price]
AS
BEGIN
    SELECT [Room].[RoomId], [Room].[Area], [Price].[Value]
	FROM [dbo].[Room] [Room]
	LEFT JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId] = [Room].[RoomTypeId]
	WHERE [Room].[Area] * [Price].[Value] > ALL(
	    SELECT [Room1].[Area] * [Price1].[Value]
		FROM [dbo].[Room] [Room1]
	    RIGHT JOIN [dbo].[Price] AS [Price1] ON [Price1].[RoomTypeId] = [Room1].[RoomTypeId]
		WHERE [Room1].[RoomId] != [Room].[RoomId]
	);
END;
GO

EXEC [dbo].[sp_Room_With_Recorded_Highest_Price];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Rooms_With_Not_Highest_Recorded_Price]
AS
BEGIN
    SELECT [Room].[RoomId], [Room].[Area], [Price].[Value]
	FROM [dbo].[Room] [Room]
	LEFT JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId] = [Room].[RoomTypeId]
	WHERE [Room].[Area] * [Price].[Value] < ANY(
	    SELECT [Room1].[Area] * [Price1].[Value]
		FROM [dbo].[Room] [Room1]
	    RIGHT JOIN [dbo].[Price] AS [Price1] ON [Price1].[RoomTypeId] = [Room1].[RoomTypeId]
		WHERE [Room1].[RoomId] != [Room].[RoomId]
	); 
END;
GO

EXEC [dbo].[sp_Rooms_With_Not_Highest_Recorded_Price];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Price_Of_Room_History]
AS
BEGIN
	SELECT [Report].[RoomId], SUM( [Report].[Value] * [Report].[Days] / 30.4375 ) AS [TotalPrice]
	FROM (
	    SELECT [Room].[RoomId], [Price].[Value], DATEDIFF(day, [Price].[StartDate], CASE WHEN [Price].[EndDate] IS NULL THEN GETDATE() ELSE [Price].[EndDate] END) AS [Days]
		FROM [dbo].[Room] [Room]
		LEFT JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId]=[Room].[RoomTypeId]
		WHERE [Price].[EndDate] IS NOT NULL
		GROUP BY [Room].[RoomId], [Price].[Value], DATEDIFF(day, [Price].[StartDate], CASE WHEN [Price].[EndDate] IS NULL THEN GETDATE() ELSE [Price].[EndDate] END)
	) AS [Report]
	GROUP BY [Report].[RoomId];
END;
GO

EXEC [dbo].[sp_Price_Of_Room_History];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Addresses_With_No_Residents]
AS
BEGIN
    SELECT [Address].[AddressId], [Address].[City], [Address].[Street], [Address].[Building]
	FROM [dbo].[Address] [Address]
	WHERE [AddressId] NOT IN(
	    SELECT [Tenant].[AddressId]
		FROM [dbo].[Tenant] [Tenant]
		UNION
		SELECT [Owner].[AddressId]
		FROM [dbo].[Owner] [Owner]
	);
END;
GO

EXEC [dbo].[sp_Addresses_With_No_Residents];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Average_Price_Of_RoomType]
AS
BEGIN
    SELECT [Report].[Name], AVG([Report].[Value]) AS [AveragePrice]
	FROM(
	    SELECT [RoomType].[Name], [Price].[Value]
		FROM [dbo].[RoomType] [RoomType]
		RIGHT JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId] = [RoomType].[RoomTypeId]
	) AS [Report]
	GROUP BY [Report].[Name];
END;
GO

EXEC [dbo].[sp_Average_Price_Of_RoomType];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Amount_Of_Residents_In_Cities]
AS
BEGIN
    SELECT * 
	FROM(SELECT [City], [TenantId] AS [ResidentId]
        FROM [dbo].[Address] [Address]
		INNER JOIN [dbo].[Tenant] AS [Tenant] On [Tenant].[AddressId] = [Address].[AddressId]
		UNION
		SELECT [City], [OwnerId] AS [ResidentId]
        FROM [dbo].[Address] [Address]
		INNER JOIN [dbo].[Owner] AS [Owner] On [Owner].[AddressId] = [Address].[AddressId]) AS [Residence] 
    PIVOT(
        COUNT([ResidentId]) 
        FOR [City] IN (
            [New York], 
            [Tokyo], 
            [Paris], 
            [London], 
            [Los Angeles])
    ) AS pivot_table;
END;
GO

EXEC [dbo].[sp_Amount_Of_Residents_In_Cities];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Update_Address_Building] @City [nvarchar](255), @SetBuilding [nvarchar](255), @Building [nvarchar](255)
AS
BEGIN
    UPDATE [dbo].[Address]
	SET [Building] = @SetBuilding, [ModifiedBy] = '00000000-0000-0000-0000-000000000001', [ModifiedDateTime] = GETDATE()
	WHERE [City] = @City AND [Building] = @Building;
END;
GO

DECLARE @SetBuilding [nvarchar](255) =  CONVERT(VARCHAR(20), RAND()), @Building [nvarchar](255);

SELECT [Address].[AddressId], [Address].[Building], [Address].[ModifiedBy], [Address].[ModifiedDateTime]
FROM [dbo].[Address] [Address];

SELECT @Building = [Address].[Building]
FROM [dbo].[Address] [Address]
WHERE [Address].[City] = 'Los Angeles';

EXEC [dbo].[sp_Update_Address_Building] @City = 'Los Angeles', @SetBuilding = @SetBuilding, @Building = @Building;

SELECT [Address].[AddressId], [Address].[Building], [Address].[ModifiedBy], [Address].[ModifiedDateTime]
FROM [dbo].[Address] [Address];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Change_Namber_Of_All_Rooms_Of_The_Specific_Owner] @OwnerId [Uniqueidentifier]
AS
BEGIN	
	UPDATE [dbo].[Room]
	SET [Number] = ROUND(((1000 - 100 - 1) * RAND() + 100), 0)
	FROM [dbo].[Room] [Room]
	RIGHT JOIN [dbo].[Asset] AS [Asset] ON [Asset].[RoomId] = [Room].[RoomId]
	WHERE [Asset].[OwnerId] = @OwnerId;
END;
GO

DECLARE @OwnerId [uniqueidentifier], @AddressId [uniqueidentifier], @RoomTypeId [int];

SELECT TOP 1 @OwnerId = [Owner].[OwnerId]
FROM [dbo].[Owner] [Owner]
WHERE [Owner].[Name] = 'Alexander Patel'

SELECT [Room].[Number], [Room].[ModifiedBy], [Room].[ModifiedDateTime]
FROM [dbo].[Room] [Room];

EXEC [dbo].[sp_Change_Namber_Of_All_Rooms_Of_The_Specific_Owner] @OwnerId = @OwnerId;

SELECT [Room].[Number], [Room].[ModifiedBy], [Room].[ModifiedDateTime]
FROM [dbo].[Room] [Room];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Insert_New_RoomType] @Name [nvarchar](20)
AS
BEGIN
    EXEC [dbo].[sp_RoomType_Insert] @Name = @Name;
END;
GO

SELECT [RoomType].[Name]
FROM [dbo].[RoomType] [RoomType];

DECLARE @Name [nvarchar](20);
SET @Name = CONCAT('Room Type', CONVERT(VARCHAR(20), ROUND(((100 - 10 - 1) * RAND() + 10), 0)));

EXEC [dbo].[sp_Insert_New_RoomType] @Name = @Name;

SELECT [RoomType].[Name]
FROM [dbo].[RoomType] [RoomType];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Insert_New Bill] @BillId [uniqueidentifier]
AS
BEGIN
    INSERT INTO [dbo].[Bill]
	SELECT NEWID(), [PrevBill].[TenantId], [PrevBill].[AssetId], [PrevBill].[BillAmount], DATEADD(DAY, 1, [PrevBill].[IssueDate]), '00000000-0000-0000-0000-000000000001', GETDATE()
	FROM(
	    SELECT *
	    FROM [dbo].[Bill] [Bill]
		WHERE [Bill].[BillId] = @BillId
	) AS [PrevBill];
END;
GO

DECLARE @BillId [uniqueidentifier];

SELECT TOP(1) @BillId = [Bill].[BillId] 
FROM [dbo].[Bill] [Bill];

SELECT [Bill].[BillId], [Bill].[IssueDate], [Bill].[AssetId], [Bill].[TenantId], [Bill].[BillAmount]
FROM [dbo].[Bill] [Bill];

EXEC [dbo].[sp_Insert_New Bill] @BillId = @BillId;

SELECT [Bill].[BillId], [Bill].[IssueDate], [Bill].[AssetId], [Bill].[TenantId], [Bill].[BillAmount]
FROM [dbo].[Bill] [Bill];

TRUNCATE TABLE [dbo].[Payment];
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Delete_Unused_Addresses]
AS
BEGIN
    BEGIN TRANSACTION;
        DELETE 
        FROM [dbo].[Address] 
        WHERE [AddressId] NOT IN(
            SELECT [Address].[AddressId]
	        FROM [dbo].[Address] [Address]
	        RIGHT JOIN [dbo].[Owner] AS [Owner] ON [Owner].[AddressId] = [Address].[AddressId]
	        UNION
	        SELECT [Address].[AddressId]
	        FROM [dbo].[Address] [Address]
	        RIGHT JOIN [dbo].[Tenant] AS [Tenant] ON [Tenant].[AddressId] = [Address].[AddressId])
	COMMIT TRANSACTION;
END;
GO