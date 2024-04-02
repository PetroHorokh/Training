IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Address_CreatedBy'
)
BEGIN
    ALTER TABLE [dbo].[Address]
	ADD CONSTRAINT [DF_Address_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Address_CreatedDateTime'
)
BEGIN
    ALTER TABLE [dbo].[Address]
	ADD CONSTRAINT [DF_Address_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Asset_CreatedBy'
)
BEGIN
    ALTER TABLE [dbo].[Asset]
	  ADD CONSTRAINT [DF_Asset_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Asset_CreatedDateTime'
)
BEGIN
    ALTER TABLE [dbo].[Asset]
	ADD CONSTRAINT [DF_Asset_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Bill_CreatedBy'
)
BEGIN
    ALTER TABLE [dbo].[Bill]
    ADD CONSTRAINT [DF_Bill_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Bill_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Bill]
	ADD CONSTRAINT [DF_Bill_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Impost_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Impost]
	  ADD CONSTRAINT [DF_Impost_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Impost_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Impost]
	ADD CONSTRAINT [DF_Impost_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Owner_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Owner]
	  ADD CONSTRAINT [DF_Owner_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Owner_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Owner]
	ADD CONSTRAINT [DF_Owner_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Payment_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Payment]
	  ADD CONSTRAINT [DF_Payment_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Payment_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Payment]
	ADD CONSTRAINT [DF_Payment_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Price_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Price]
	  ADD CONSTRAINT [DF_Price_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Price_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Price]
	ADD CONSTRAINT [DF_Price_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Rent_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Rent]
	  ADD CONSTRAINT [DF_Rent_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Rent_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Rent]
	ADD CONSTRAINT [DF_Rent_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Room_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Room]
	  ADD CONSTRAINT [DF_Room_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Room_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Room]
	ADD CONSTRAINT [DF_Room_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_RoomType_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[RoomType]
	  ADD CONSTRAINT [DF_RoomType_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_RoomType_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[RoomType]
	ADD CONSTRAINT [DF_RoomType_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Tenant_CreatedBy'
)
BEGIN
	  ALTER TABLE [dbo].[Tenant]
	  ADD CONSTRAINT [DF_Tenant_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Tenant_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Tenant]
	ADD CONSTRAINT [DF_Tenant_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Accommodation_CreatedBy'
)
BEGIN
	ALTER TABLE [dbo].[Accommodation]
	ADD CONSTRAINT [DF_Accommodation_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Accommodation_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Accommodation]
	ADD CONSTRAINT [DF_Accommodation_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_AccommodationRoom_CreatedBy'
)
BEGIN
	ALTER TABLE [dbo].[AccommodationRoom]
	ADD CONSTRAINT [DF_AccommodationRoom_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_AccommodationRoom_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[AccommodationRoom]
	ADD CONSTRAINT [DF_AccommodationRoom_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_User_CreatedBy'
)
BEGIN
    ALTER TABLE [dbo].[User]
	ADD CONSTRAINT [DF_User_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_User_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[User]
	ADD CONSTRAINT [DF_User_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Role_CreatedBy'
)
BEGIN
	ALTER TABLE [dbo].[Role]
	ADD CONSTRAINT [DF_Role_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Role_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Role]
	ADD CONSTRAINT [DF_Role_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_UserRole_CreatedBy'
)
BEGIN
	ALTER TABLE [dbo].[UserRole]
	ADD CONSTRAINT [DF_UserRole_CreatedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [CreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_UserRole_CreatedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[UserRole]
	ADD CONSTRAINT [DF_UserRole_CreatedDateTime] DEFAULT GETDATE() FOR [CreatedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Address_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Address]
	  ADD CONSTRAINT [DF_Address_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Address_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Address]
	ADD CONSTRAINT [DF_Address_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Asset_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Asset]
    ADD CONSTRAINT [DF_Asset_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Asset_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Asset]
	ADD CONSTRAINT [DF_Asset_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Bill_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Bill]
	  ADD CONSTRAINT [DF_Bill_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Bill_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Bill]
	ADD CONSTRAINT [DF_Bill_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Impost_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Impost]
	  ADD CONSTRAINT [DF_Impost_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Impost_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Impost]
	ADD CONSTRAINT [DF_Impost_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Owner_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Owner]
	  ADD CONSTRAINT [DF_Owner_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Owner_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Owner]
	ADD CONSTRAINT [DF_Owner_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Payment_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Payment]
	  ADD CONSTRAINT [DF_Payment_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Payment_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Payment]
	ADD CONSTRAINT [DF_Payment_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Price_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Price]
	  ADD CONSTRAINT [DF_Price_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Price_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Price]
	ADD CONSTRAINT [DF_Price_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Rent_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Rent]
	  ADD CONSTRAINT [DF_Rent_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Rent_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Rent]
	ADD CONSTRAINT [DF_Rent_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Room_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Room]
	  ADD CONSTRAINT [DF_Room_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Room_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Room]
	ADD CONSTRAINT [DF_Room_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_RoomType_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[RoomType]
	  ADD CONSTRAINT [DF_RoomType_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_RoomType_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[RoomType]
	ADD CONSTRAINT [DF_RoomType_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Tenant_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Tenant]
	  ADD CONSTRAINT [DF_Tenant_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Tenant_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Tenant]
	ADD CONSTRAINT [DF_Tenant_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Accommodation_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Accommodation]
	ADD CONSTRAINT [DF_Accommodation_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Accommodation_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Accommodation]
	ADD CONSTRAINT [DF_Accommodation_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_AccommodationRoom_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[AccommodationRoom]
	ADD CONSTRAINT [DF_AccommodationRoom_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_AccommodationRoom_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[AccommodationRoom]
	ADD CONSTRAINT [DF_AccommodationRoom_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_User_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[User]
	ADD CONSTRAINT [DF_User_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_User_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[User]
	ADD CONSTRAINT [DF_User_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Role_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[Role]
	ADD CONSTRAINT [DF_Role_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_Role_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[Role]
	ADD CONSTRAINT [DF_Role_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_UserRole_ModifiedBy'
)
BEGIN
    ALTER TABLE [dbo].[UserRole]
	ADD CONSTRAINT [DF_UserRole_ModifiedBy] DEFAULT CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) FOR [ModifiedBy];
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [sys].[default_constraints]
    WHERE [name]  = 'DF_UserRole_ModifiedDateTime'
)
BEGIN
	ALTER TABLE [dbo].[UserRole]
	ADD CONSTRAINT [DF_UserRole_ModifiedDateTime] DEFAULT GETDATE() FOR [ModifiedDateTime];
END;
GO