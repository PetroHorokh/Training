IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Rent')
CREATE DATABASE Rent;
GO

USE [Rent];
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Accommodation' and xtype='U')
CREATE TABLE [dbo].[Accommodation](
	[AccommodationId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDateTime] [datetime2] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[ModifiedDateTime] [datetime2] NOT NULL,
	CONSTRAINT [PK_Accommodation] PRIMARY KEY ([AccommodationId])
);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Address' and xtype='U')
CREATE TABLE [dbo].[Address](
    [AddressId] [uniqueidentifier] NOT NULL,
    [City] [nvarchar](255) NOT NULL,
    [Street] [nvarchar](255) NOT NULL,
    [Building] [nvarchar](255) NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
	CONSTRAINT [PK_Address] PRIMARY KEY ([AddressId])
)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='RoomType' and xtype='U')
CREATE TABLE [dbo].[RoomType](
    [RoomTypeId] [int] NOT NULL,
    [Name] [nvarchar](20)  NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_RoomType] PRIMARY KEY ([RoomTypeId])
)
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Price' and xtype='U')
CREATE TABLE [dbo].[Price](
    [PriceId] [uniqueidentifier] NOT NULL,
    [StartDate] [datetime2] NOT NULL,
    [Value] [numeric](18,2) NOT NULL,
    [EndDate] [datetime2] NULL,
    [RoomTypeId] [int] NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
	[ValidFrom] [datetime2] GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    [ValidTo] [datetime2] GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo]),
	CONSTRAINT [PK_Price] PRIMARY KEY ([PriceId]),
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[Price_History])
);
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Price_RoomTypeId_RoomType_RoomTypeId'
)
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_RoomTypeId_RoomType_RoomTypeId] FOREIGN KEY([RoomTypeId])
REFERENCES [dbo].[RoomType] ([RoomTypeId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Room' and xtype='U')
CREATE TABLE [dbo].[Room](
    [RoomId] [uniqueidentifier] NOT NULL,
    [Number] [int] NOT NULL,
    [Area] [numeric](18, 2)  NOT NULL,
    [RoomTypeId] [int] NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Room] PRIMARY KEY ([RoomId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Room_RoomTypeId_RoomType_RoomTypeId'
)
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_RoomTypeId_RoomType_RoomTypeId] FOREIGN KEY([RoomTypeId])
REFERENCES [dbo].[RoomType] ([RoomTypeId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AccommodationRoom' and xtype='U')
CREATE TABLE [dbo].[AccommodationRoom](
	[AccommodationRoomId] [uniqueidentifier] NOT NULL,
	[AccommodationId] [int] NOT NULL,
	[RoomId] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDateTime] [datetime2] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[ModifiedDateTime] [datetime2] NOT NULL,
	CONSTRAINT [PK_AccommodationRoom] PRIMARY KEY ([AccommodationRoomId])
);
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_AccommodationRoom_AccommodationId_Accommodation_AccommodationId'
)
ALTER TABLE [dbo].[AccommodationRoom]  WITH CHECK ADD  CONSTRAINT [FK_AccommodationRoom_AccommodationId_Accommodation_AccommodationId] FOREIGN KEY([AccommodationId])
REFERENCES [dbo].[Accommodation] ([AccommodationId]);
GO

IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_AccommodationRoom_RoomId_Room_RoomId'
)
ALTER TABLE [dbo].[AccommodationRoom]  WITH CHECK ADD  CONSTRAINT [FK_AccommodationRoom_RoomId_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([RoomId]);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Owner' and xtype='U')
CREATE TABLE [dbo].[Owner](
    [OwnerId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [AddressId] [uniqueidentifier] NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Owner] PRIMARY KEY ([OwnerId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Owner_AddressId_Address_AddressId'
)
ALTER TABLE [dbo].[Owner]  WITH CHECK ADD  CONSTRAINT [FK_Owner_AddressId_Address_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Asset' and xtype='U')
CREATE TABLE [dbo].[Asset](
    [AssetId] [uniqueidentifier] NOT NULL,
    [OwnerId] [uniqueidentifier] NOT NULL,
    [RoomId] [uniqueidentifier] NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_OwnerShip] PRIMARY KEY([AssetId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Asset_OwnerId_Owner_OwnerId'
)
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_OwnerId_Owner_OwnerId] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Owner] ([OwnerId])
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Asset_RoomId_Room_RoomId'
)
ALTER TABLE [dbo].[Asset]  WITH CHECK ADD  CONSTRAINT [FK_Asset_RoomId_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Room] ([RoomId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tenant' and xtype='U')
CREATE TABLE [dbo].[Tenant](
    [TenantId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [BankName] [nvarchar](50) NOT NULL,
    [AddressId] [uniqueidentifier] NOT NULL,
    [Director] [nvarchar](50) NOT NULL,
    [Description] [nvarchar](255) NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Tenant] PRIMARY KEY([TenantId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Tenant_AddressId_Address_AddressId'
)
ALTER TABLE [dbo].[Tenant]  WITH CHECK ADD  CONSTRAINT [FK_Tenant_AddressId_Address_AddressId] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([AddressId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Bill' and xtype='U')
CREATE TABLE [dbo].[Bill](
    [BillId] [uniqueidentifier] NOT NULL,
    [TenantId] [uniqueidentifier] NOT NULL,
    [RentId] [uniqueidentifier] NOT NULL,
    [BillAmount] [numeric](18, 2) NOT NULL,
    [IssueDate] [datetime2] NOT NULL,
    [EndDate] [datetime2] NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Bill] PRIMARY KEY ([BillId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Bill_TenantId_Tenant_TenantId'
)
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_TenantId_Tenant_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Bill_RentId_Rent_RentId'
)
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_RentId_Rent_RentId] FOREIGN KEY([RentId])
REFERENCES [dbo].[Rent] ([RentId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Rent' and xtype='U')
CREATE TABLE [dbo].[Rent](
    [RentId] [uniqueidentifier] NOT NULL,
    [AssetId] [uniqueidentifier] NOT NULL,
    [TenantId] [uniqueidentifier] NOT NULL,
    [StartDate] [datetime2] NOT NULL,
    [EndDate] [datetime2] NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Rent] PRIMARY KEY ([RentId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Rent_AssetId_Asset_AssetId'
)
ALTER TABLE [dbo].[Rent]  WITH CHECK ADD  CONSTRAINT [FK_Rent_AssetId_Asset_AssetId] FOREIGN KEY([AssetId])
REFERENCES [dbo].[Asset] ([AssetId])
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Rent_TenantId_Tenant_TenantId'
)
ALTER TABLE [dbo].[Rent]  WITH CHECK ADD  CONSTRAINT [FK_Rent_TenantId_Tenant_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Impost' and xtype='U')
CREATE TABLE [dbo].[Impost](
    [ImpostId] [uniqueidentifier] NOT NULL,
    [Tax] [numeric](4,2) NOT NULL,
    [Fine] [numeric](3,2) NOT NULL,
    [PaymentDay] [int] NOT NULL,
    [StartDate] [datetime2] NOT NULL,
    [EndDate] [datetime2] NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
	[ValidFrom] [datetime2] GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    [ValidTo] [datetime2] GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo]),
    CONSTRAINT [PK_Impost] PRIMARY KEY ([ImpostId])
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[Impost_History])
);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Payment' and xtype='U')
CREATE TABLE [dbo].[Payment](
    [PaymentId] [uniqueidentifier] NOT NULL,
    [TenantId] [uniqueidentifier] NOT NULL,
    [BillId] [uniqueidentifier] NOT NULL,
    [PaymentDay] [datetime2] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
    [CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Payment] PRIMARY KEY ([PaymentId])
)
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Payment_BillId_Bill_BillId'
)
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_BillId_Bill_BillId] FOREIGN KEY([BillId])
REFERENCES [dbo].[Bill] ([BillId])
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_Payment_TenantId_Tenant_TenantId'
)
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_TenantId_Tenant_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='User' and xtype='U')
CREATE TABLE [dbo].[User](
    [UserId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([UserId]),
	ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[User_History])
);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Role' and xtype='U')
CREATE TABLE [dbo].[Role](
    [RoleId] [int] NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([RoleId]),
	ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[Role_History])
);
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='UserRole' and xtype='U')
CREATE TABLE [dbo].UserRole(
    [UserRoleId] [uniqueidentifier] NOT NULL,
    [UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [int] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
    [CreatedDateTime] [datetime2] NOT NULL,
    [ModifiedBy] [uniqueidentifier] NOT NULL,
    [ModifiedDateTime] [datetime2] NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY ([UserRoleId]),
	ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserRole_History])
);
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_UserRole_UserId_User_UserId'
)
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_UserId_User_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId]);
GO
IF NOT EXISTS(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_UserRole_RoleId_Role_RoleId'
)
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_RoleId_Role_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId]);
GO