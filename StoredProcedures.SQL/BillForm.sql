CREATE OR ALTER PROCEDURE [dbo].[sp_Bill_Month]
AS
BEGIN
	CREATE TABLE #TempRentIds
    ([RentId] [uniqueidentifier]);

	DECLARE 
		@StartDate [datetime2] = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1), 
		@EndDate [datetime2] = EOMONTH(GETDATE());	

	INSERT INTO [#TempRentIds]([RentId])
	SELECT [Rent].[RentId]
	FROM [dbo].[Rent] [Rent]
	WHERE
		@StartDate BETWEEN [Rent].[StartDate] AND [Rent].[EndDate];

	INSERT INTO [dbo].[Bill]([BillId],[TenantId],[RentId],[BillAmount],[IssueDate])
	SELECT NEWID(), 
		[Data].[TenantId], 
		[Data].[RentId], 
		[Data].[Area] * [Data].[Price] * DATEDIFF(DAY, [Data].[IssueDate], [Data].[Days]) / DAY(EOMONTH(GETDATE())) AS [BillAmount],
		[Data].[IssueDate]
	FROM(SELECT [Rent].[RentId],
		[Rent].[AssetId],
		[Rent].[TenantId],
		@StartDate AS [IssueDate],
		CASE WHEN [Rent].[EndDate] < @EndDate THEN [Rent].[EndDate] ELSE @EndDate END AS [Days],
		[Room].[Area],
		[Price].[Value] AS [Price]
		FROM [dbo].[Rent] [Rent]
		LEFT JOIN [dbo].[Asset] AS [Asset] ON [Asset].[AssetId] = [Rent].[AssetId]
		LEFT JOIN [dbo].[Room] AS [Room] ON [Room].[RoomId] = [Asset].[RoomId]
		LEFT JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId] = [Room].[RoomTypeId]
		WHERE [Rent].[RentId] IN (SELECT [RentId] FROM [#TempRentIds])
			AND [Price].[EndDate] IS NULL
	) AS [Data]

	DROP TABLE #TempRentIds;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Bill_Fine]
AS
BEGIN
	CREATE TABLE #TempBillToUpdate
    ([BillToUpdateId] [uniqueidentifier],
	[Amount] [numeric](18,2),
	[Fine] [numeric](18,2));

	INSERT INTO #TempBillToUpdate
	SELECT [Bill].[BillId], [Bill].[BillAmount], [Impost].[Fine]
	FROM [dbo].[Bill] [Bill]
	INNER JOIN [dbo].[Impost] AS [Impost] ON [Bill].[IssueDate] BETWEEN [Impost].[StartDate] AND [Impost].[EndDate]
	WHERE [Bill].[EndDate] IS NULL AND DATEDIFF(day, [Bill].[IssueDate], CAST(GETDATE() AS DATE)) > [Impost].[PaymentDay];


	UPDATE [dbo].[Bill]
	SET [BillAmount] = [Amount] * (1 + [Fine])
	FROM [#TempBillToUpdate]
	WHERE [BillId] = [BillToUpdateId]

	DROP TABLE #TempBillToUpdate;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Bill_Tax]
AS
BEGIN
	CREATE TABLE #TempBillToUpdate
    ([BillToUpdateId] [uniqueidentifier],
	[Amount] [numeric](18,2),
	[Tax] [numeric](18,2));

	INSERT INTO #TempBillToUpdate
	SELECT [Bill].[BillId], [Bill].[BillAmount], [Impost].[Tax]
	FROM [dbo].[Bill] [Bill]
	INNER JOIN [dbo].[Impost] AS [Impost] ON [Bill].[IssueDate] BETWEEN [Impost].[StartDate] AND [Impost].[EndDate]
	WHERE [Bill].[EndDate] IS NULL AND DATEDIFF(day, [Bill].[IssueDate], CAST(GETDATE() AS DATE)) = [Impost].[PaymentDay];


	UPDATE [dbo].[Bill]
	SET [BillAmount] = [Amount] * (1 + [Tax])
	FROM [#TempBillToUpdate]
	WHERE [BillId] = [BillToUpdateId]

	DROP TABLE #TempBillToUpdate;
END;
GO