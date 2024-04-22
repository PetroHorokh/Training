CREATE OR ALTER TRIGGER [dbo].[tr_Address_Insert]
ON [dbo].[Address]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Address]([AddressId],[City],[Street],[Building],[CreatedBy],[CreatedDateTime])
			SELECT i.[AddressId], i.[City], i.[Street], i.[Building], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE() AS [CreatedDateTime]
			FROM inserted i;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Asset_Insert]
ON [dbo].[Asset]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Asset]([AssetId],[OwnerId],[RoomId],[CreatedBy],[CreatedDateTime])
			SELECT [AssetId], [OwnerId], [RoomId], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Bill_Insert]
ON [dbo].[Bill]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Bill]([BillId],[TenantId],[RentId],[BillAmount],[IssueDate],[CreatedBy],[CreatedDateTime])
			SELECT [BillId],[TenantId],[RentId],[BillAmount],[IssueDate], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Owner_Insert]
ON [dbo].[Owner]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Owner]([OwnerId],[UserId],[Name],[AddressId],[CreatedBy],[CreatedDateTime])
			SELECT [OwnerId],[UserId],[Name],[AddressId], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Payment_Insert]
ON [dbo].[Payment]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Payment]([PaymentId],[TenantId],[BillId],[PaymentDay],[Amount],[CreatedBy],[CreatedDateTime])
			SELECT [PaymentId],[TenantId],[BillId],GETDATE(),[Amount], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Rent_Insert]
ON [dbo].[Rent]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Rent]([RentId],[AssetId],[TenantId],[StartDate],[EndDate],[CreatedBy],[CreatedDateTime])
			SELECT [RentId],[AssetId],[TenantId],[StartDate],[EndDate], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Room_Insert]
ON [dbo].[Room]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Room]([RoomId],[AddressId],[Number],[Area],[RoomTypeId],[CreatedBy],[CreatedDateTime])
			SELECT [RoomId],[AddressId],[Number],[Area],[RoomTypeId], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_RoomType_Insert]
ON [dbo].[RoomType]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[RoomType]([RoomTypeId],[Name],[CreatedBy],[CreatedDateTime])
			SELECT [RoomTypeId],[Name], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
            ROLLBACK TRANSACTION;
            DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Tenant_Insert]
ON [dbo].[Tenant]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Tenant]([TenantId],[UserId],[Name],[BankName],[AddressId],[Director],[Description],[CreatedBy],[CreatedDateTime])
			SELECT [TenantId],[UserId],[Name],[BankName],[AddressId],[Director],[Description], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())) AS [CreatedBy], GETDATE()  AS [CreatedDateTime]
			FROM inserted i;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Accommodation_Insert]
ON [dbo].[Accommodation]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[Accommodation]([AccommodationId],[Name],[CreatedBy],[CreatedDateTime])
			SELECT [AccommodationId], [Name], i.[CreatedBy], GETDATE() AS [CreatedDateTime]
			FROM inserted i;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_AccommodationRoom_Insert]
ON [dbo].[AccommodationRoom]
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			INSERT INTO [dbo].[AccommodationRoom]([AccommodationRoomId],[AccommodationId],[RoomId], [Quantity],[CreatedBy],[CreatedDateTime])
			SELECT [AccommodationRoomId], [AccommodationId], [RoomId], i.[Quantity], CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), GETDATE() AS [CreatedDateTime]
			FROM inserted i;
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Address_Update]
ON [dbo].[Address]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Address]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [AddressId] = (SELECT [AddressId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Asset_Update]
ON [dbo].[Asset]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Asset]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [AssetId] = (SELECT [AssetId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Bill_Update]
ON [dbo].[Bill]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Bill]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [BillId] = (SELECT [BillId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Owner_Update]
ON [dbo].[Owner]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Owner]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [OwnerId] = (SELECT [OwnerId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Payment_Update]
ON [dbo].[Payment]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Payment]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [PaymentId] = (SELECT [PaymentId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Rent_Update]
ON [dbo].[Rent]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Rent]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [RentId] = (SELECT [RentId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Room_Update]
ON [dbo].[Room]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Room]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [RoomId] = (SELECT [RoomId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_RoomType_Update]
ON [dbo].[RoomType]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[RoomType]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [RoomTypeId] = (SELECT [RoomTypeId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Tenant_Update]
ON [dbo].[Tenant]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[Tenant]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [TenantId] = (SELECT [TenantId] FROM inserted);
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_Accommodation_Update]
ON [dbo].[Accommodation]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	  BEGIN TRANSACTION;
			UPDATE [dbo].[Accommodation]
      SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [AccommodationId] = (SELECT [AccommodationId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

CREATE OR ALTER TRIGGER [dbo].[tr_AccommodationRoom_Update]
ON [dbo].[AccommodationRoom]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
	BEGIN TRY;
	    BEGIN TRANSACTION;
			UPDATE [dbo].[AccommodationRoom]
			SET [ModifiedBy] = CONVERT(UNIQUEIDENTIFIER, CONVERT(BINARY(16), SUSER_SID())), [ModifiedDateTime] = GETDATE()
			WHERE [AccommodationRoomId] = (SELECT [AccommodationRoomId] FROM inserted)
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 0);
		END;
    END CATCH;
END;
GO

