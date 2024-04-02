CREATE OR ALTER VIEW [dbo].[vw_Rooms_With_Tenants]
AS
SELECT [Room].[Number], [Tenant].[Name], [Rent].[StartDate], [Rent].[EndDate]
FROM [dbo].[Room] [Room]
LEFT JOIN [dbo].[Asset] AS [Asset] ON [Asset].[RoomId] = [Room].[RoomId]
LEFT JOIN [dbo].[Rent] AS [Rent] ON [Rent].[AssetId] = [Asset].[AssetId]
LEFT JOIN [dbo].[Tenant] AS [Tenant] ON [Tenant].[TenantId] = [Rent].[TenantId];
GO

SELECT [RWT].[Number] AS [Room Number], [RWT].[Name] AS [Tenant Name]
FROM [dbo].[vw_Rooms_With_Tenants] [RWT]
WHERE '2022-01-02' BETWEEN [RWT].[StartDate] AND [RWT].[EndDate];
GO

CREATE OR ALTER VIEW [dbo].[vw_Certificate_For_Tenant]
AS
SELECT [Tenant].[TenantId], [Rent].[RentId], [Rent].[StartDate] AS [Rent Start Date], [Rent].[EndDate] AS [Rent End Date], [Bill].[BillId], [Payment].[PaymentId]
FROM [dbo].[Tenant] [Tenant]
RIGHT JOIN [dbo].[Rent] AS [Rent] ON [Rent].[TenantId] = [Tenant].[TenantId]
LEFT JOIN [dbo].[Bill] AS [Bill] ON [Bill].[RentId] = [Rent].[RentId]
LEFT JOIN [dbo].[Payment] AS [Payment] ON [Payment].[BillId] = [Bill].[BillId]
WHERE [Bill].[IssueDate] BETWEEN [Rent].[StartDate] AND [Rent].[EndDate];
GO

DECLARE @TenantId [uniqueidentifier];

SELECT @TenantId = [Tenant].[TenantId]
FROM [dbo].[Tenant] [Tenant]
WHERE [Tenant].[Name] = 'Widget Industries';

SELECT *
FROM [dbo].[vw_Certificate_For_Tenant] [CFT]
WHERE [CFT].[TenantId] = @TenantId 
GO

CREATE OR ALTER VIEW [dbo].[vw_Tenant_Asset_Payment]
AS
SELECT [Tenant].[TenantId], [Tenant].[Name], [Rent].[RentId], [Room].[Number], [Room].[Area] * [Price].[Value] AS [Price]
FROM [dbo].[Tenant] [Tenant]
INNER JOIN [dbo].[Rent] AS [Rent] ON [Rent].[TenantId] = [Tenant].[TenantId]
LEFT JOIN [dbo].[Asset] AS [Asset] ON [Asset].[AssetId] = [Rent].[AssetId]
LEFT JOIN [dbo].[Room] AS [Room] ON [Room].[RoomId] = [Asset].[RoomId]
INNER JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId] = [Room].[RoomTypeId]
WHERE [Price].[StartDate] BETWEEN [Rent].[StartDate] AND [Rent].[EndDate]
GO

SELECT *
FROM [vw_Tenant_Asset_Payment];
GO