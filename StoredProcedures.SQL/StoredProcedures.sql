CREATE OR ALTER PROCEDURE [dbo].[sp_Address_Insert]
@City [nvarchar](255),
@Street [nvarchar](255),
@Building [nvarchar](255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF EXISTS (SELECT 1
		    FROM [dbo].[Address] [Address]
		    WHERE [Address].[City] = @City
                AND [Address].[Street] = @Street
                AND [Address].[Building] = @Building)
		BEGIN
            RAISERROR( 'There is aleady such address' , 11, 3) WITH NOWAIT;
	    END;

		DECLARE @AddressId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Address] ([AddressId], [City], [Street], [Building])
		SELECT @AddressId, @City, @Street, @Building;
		SELECT @AddressId AS [AddressId];
    END TRY
    BEGIN CATCH
        DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
        RAISERROR( @Message , 11, 3);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Owner_Insert]
@AddressId [uniqueidentifier],
@Name [nvarchar](50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF NOT EXISTS(SELECT 1
			FROM [dbo].[Address]
			WHERE [AddressId] = @AddressId
		)
		BEGIN
			RAISERROR('There is no such address' ,11 ,7);
		END;

		DECLARE @OwnerId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Owner] ([OwnerId], [Name], [AddressId])
		SELECT @OwnerId, @Name, @AddressId;
		SELECT @OwnerId AS [OwnerId];

    END TRY
    BEGIN CATCH
        DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
        RAISERROR( @Message , 11, 7);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Tenant_Insert]
@AddressId [uniqueidentifier],
@Name [nvarchar](50),
@BankName [nvarchar](50),
@Director [nvarchar](50),
@Description [nvarchar](255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF NOT EXISTS(SELECT 1 
			FROM [dbo].[Address] [Address]
			WHERE [Address].[AddressId] = @AddressId)
		BEGIN
			RAISERROR('There is no such address', 11, 13);
		END;

		DECLARE @TenantId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Tenant] ([TenantId], [Name], [BankName], [AddressId], [Director], [Description])
		SELECT @TenantId, @Name, @BankName, @AddressId, @Director, @Description;
		SELECT @TenantId AS [TenantId];
    END TRY
    BEGIN CATCH
        DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
        RAISERROR( @Message , 11, 13);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_RoomType_Insert]
@Name [nvarchar](20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	    DECLARE @RoomTypeId [int]; 

	    IF EXISTS (SELECT 1 
		    FROM [dbo].[RoomType] [RoomType]
			WHERE [RoomType].[Name] = @Name)
        BEGIN
		    RAISERROR( 'There already is such room type' , 11, 13) WITH NOWAIT;
        END;

		WITH Ids(Id) AS
			(
				SELECT Id = 1
				UNION ALL
				SELECT Id + 1
				FROM Ids
				WHERE Id IN(SELECT [RoomTypeId] FROM [dbo].[RoomType])
			)
			SELECT TOP 1 @RoomTypeId = Id
			FROM Ids
			ORDER BY [id] DESC;

		INSERT INTO [dbo].[RoomType] ([RoomTypeId], [Name])
		SELECT @RoomTypeId, @Name;
		SELECT @RoomTypeId AS [RoomTypeId];
    END TRY
    BEGIN CATCH
        DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 13);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Price_Insert]
@Value [numeric](18,2),
@RoomTypeId [int]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY	
		IF NOT EXISTS(SELECT 1
			FROM [dbo].[RoomType]
			WHERE [RoomTypeId] = @RoomTypeId
		)
		BEGIN
			RAISERROR( 'There is no such room type' , 11, 9) WITH NOWAIT;
		END;

		BEGIN TRANSACTION;
			UPDATE [dbo].[Price]
			SET [EndDate] = GETDATE()
			WHERE [RoomTypeId] = @RoomTypeId AND [EndDate] IS NULL;
		COMMIT TRANSACTION;

		DECLARE @PriceId [uniqueidentifier] = NEWID();
		BEGIN TRANSACTION;
		    INSERT INTO [dbo].[Price] ([PriceId], [StartDate], [Value], [RoomTypeId])
		    SELECT @PriceId, GETDATE(), @Value, @RoomTypeId;
		COMMIT TRANSACTION;
		SELECT @PriceId AS [PriceId];
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
			RAISERROR( @Message , 11, 9);
		END
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Room_Insert]
@Number [int],
@Area [numeric](18,2),
@RoomTypeId [nvarchar](20)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF EXISTS(
			SELECT 1
			FROM [dbo].[Room] [Room]
			WHERE [Room].[Number] = @Number
		)
		BEGIN
		    RAISERROR( 'There already is a room with such number' , 11, 12) WITH NOWAIT;
		END;

		IF NOT EXISTS(SELECT 1
		    FROM [dbo].[RoomType] [RoomType]
			WHERE [RoomType].[RoomTypeId] = @RoomTypeId
		)
		BEGIN
		    RAISERROR( 'There is no such room type' , 11, 12) WITH NOWAIT;
		END;

		DECLARE @RoomId [uniqueidentifier];
		SELECT @RoomId = NEWID();
		INSERT INTO [dbo].[Room] ([RoomId], [Number], [Area], [RoomTypeId])
		SELECT @RoomId, @Number, @Area, @RoomTypeId;
		SELECT @RoomId AS [RoomId];
    END TRY
    BEGIN CATCH
		DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 12);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Asset_Insert]
@OwnerId [uniqueidentifier],
@RoomId [uniqueidentifier]
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		
		IF NOT EXISTS(SELECT 1
			FROM [dbo].[Owner]
			WHERE [OwnerId] = @OwnerId
		)
		BEGIN
			RAISERROR( 'There is no such owner' , 11, 4) WITH NOWAIT;
		END

		IF EXISTS(SELECT 1
			FROM [dbo].[Asset] [Asset]
		    WHERE [Asset].[RoomId] = @RoomId
	    )
		BEGIN
		    RAISERROR( 'Room already belongs to someone' , 11, 4) WITH NOWAIT;
		END;

		IF NOT EXISTS(SELECT 1
			FROM [dbo].[Room]
			WHERE [RoomId] = @RoomId
		)
		BEGIN
			RAISERROR( 'There is no such room' , 11, 4) WITH NOWAIT;
		END

		DECLARE @AssetId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Asset] ([AssetId], [OwnerId], [RoomId])
		SELECT @AssetId, @OwnerId, @RoomId;
		SELECT @AssetId AS [AssetId]
    END TRY
    BEGIN CATCH
		DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 1);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Bill_Insert]
@TenantId [uniqueidentifier],
@RentId [uniqueidentifier],
@Amount [numeric](18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	    IF NOT EXISTS(SELECT 1
		    FROM [dbo].[Tenant] [Tenant]
			WHERE [Tenant].[TenantId] = @TenantId
		)
		BEGIN
		    RAISERROR( 'There is no such tenant', 11, 5) WITH NOWAIT;
		END;
		
		IF NOT EXISTS(SELECT 1
		    FROM [dbo].[Rent] [Rent]
			WHERE [Rent].[RentId] = @RentId
		)
		BEGIN
            RAISERROR( 'There is no such asset', 11, 5) WITH NOWAIT;
		END;
			
		DECLARE @BillId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Bill] ([BillId], [TenantId], [RentId], [BillAmount])
		SELECT @BillId, @TenantId, @RentId, @Amount;
		SELECT @BillId AS [BillId];
    END TRY
    BEGIN CATCH
		    DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 5);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Impost_Insert]
@Tax [decimal](4,2),
@Fine [decimal](3,2),
@PaymentDay [int],
@StartDay [datetime2],
@EndDay [datetime2]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	    
		BEGIN TRANSACTION;
			UPDATE [dbo].[Impost]
			SET [EndDate] = GETDATE()
			WHERE [EndDate] IS NULL;
		COMMIT TRANSACTION;

		DECLARE @ImpostId [uniqueidentifier] = NEWID();
		BEGIN TRANSACTION;
			INSERT INTO [dbo].[Impost] ([ImpostId], [Tax], [Fine], [PaymentDay], [StartDate])
			SELECT @ImpostId, @Tax, @Fine, @PaymentDay, GETDATE();
		COMMIT TRANSACTION;
		SELECT @ImpostId AS [ImpostId];
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE()
            RAISERROR( @Message , 11, 6);
		END
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Payment_Insert]
@TenantId [uniqueidentifier],
@BillId [uniqueidentifier],
@Amount [numeric](18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY		
		IF NOT EXISTS(SELECT 1
		FROM [dbo].[Bill] [Bill]
		WHERE [Bill].[BillId] = @BillId
		)
		BEGIN
			RAISERROR( 'There is no such bill', 11, 8) WITH NOWAIT;
		END;

		IF NOT EXISTS(SELECT 1
		FROM [dbo].[Tenant]
		WHERE [TenantId] = @TenantId
		)
		BEGIN
			RAISERROR( 'There is no such tenant', 11, 8) WITH NOWAIT;
		END;

		DECLARE @PaymentId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Payment] ([PaymentId], [TenantId], [BillId], [Amount])
		SELECT @PaymentId, @TenantId, @BillId, @Amount;
		SELECT @PaymentId AS [PaymentId];
    END TRY
    BEGIN CATCH
		    DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 8);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Rent_Insert]
@AssetId [uniqueidentifier],
@TenantId [uniqueidentifier],
@StartDate [date],
@EndDate [date]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF NOT EXISTS(SELECT 1
		    FROM [dbo].[Asset] [Asset]
			WHERE [Asset].[AssetId] = @AssetId
		)
		BEGIN
		    RAISERROR( 'There is no such asset', 11, 10) WITH NOWAIT;
		END;
		
		IF NOT EXISTS(SELECT 1
		    FROM [dbo].[Tenant] [Tenant]
			WHERE [Tenant].[TenantId] = @TenantId
		)
		BEGIN
		    RAISERROR( 'There is no such tenant', 11, 10) WITH NOWAIT;
		END;

		IF EXISTS(SELECT 1
			FROM [dbo].[Rent] [Rent] 
			WHERE @StartDate BETWEEN [Rent].[StartDate] AND [Rent].[EndDate] AND @AssetId = [Rent].[AssetId]
		)
		BEGIN
			RAISERROR( 'The room in this dates is taken', 11, 10) WITH NOWAIT;
		END;
		
		DECLARE @RentId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Rent] ([RentId], [AssetId], [TenantId], [StartDate], [EndDate])
		SELECT @RentId, @AssetId, @TenantId, @StartDate, @EndDate;
		SELECT @RentId AS [RentId];
    END TRY
    BEGIN CATCH
		DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 10);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Accommodation_Insert]
@Name [nvarchar](255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF EXISTS(SELECT 1
			FROM [dbo].[Accommodation] [Accommodation] 
			WHERE [Accommodation].[Name] = @Name
		)
		BEGIN
			RAISERROR( 'Such accommotation exists', 11, 1) WITH NOWAIT;
		END
		ELSE
		BEGIN
			DECLARE @AccommodationId [int];

			WITH Ids(Id) AS
			(
				SELECT Id = 1
				UNION ALL
				SELECT Id + 1
				FROM Ids
				WHERE Id IN(SELECT [AccommodationId] FROM [dbo].[Accommodation])
			)
			SELECT TOP 1 @AccommodationId = Id
			FROM Ids
			ORDER BY [id] DESC;
			
			INSERT INTO [dbo].[Accommodation] ([AccommodationId], [Name])
			SELECT @AccommodationId, @Name;

			SELECT @AccommodationId AS [AccommodationId];
		END
    END TRY
    BEGIN CATCH
		DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 1);
    END CATCH;
    END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_AccommodationRoom_Insert]
@AccommodationId [int],
@RoomId [uniqueidentifier],
@Quantity [int]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF NOT EXISTS(SELECT 1
			FROM [dbo].[Accommodation]
			WHERE [AccommodationId] = @AccommodationId
		)
		BEGIN
			RAISERROR('There is no such accommodation' , 11, 2);
		END;

		IF NOT EXISTS(SELECT 1
			FROM [dbo].[Room]
			WHERE [RoomId] = @RoomId
		)
		BEGIN
			RAISERROR('There is no such room' , 11, 2);
		END;

		DECLARE @AccommodationRoomId [uniqueidentifier] = NEWID();
		BEGIN TRANSACTION;
			INSERT INTO [dbo].[AccommodationRoom] ([AccommodationRoomId], [AccommodationId], [RoomId], [Quantity])
			SELECT @AccommodationRoomId, @AccommodationId, @RoomId, @Quantity;
		COMMIT TRANSACTION;
		SELECT @AccommodationRoomId AS [AccommodationRoomId];
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION;
			DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
            RAISERROR( @Message , 11, 2);
		END
    END CATCH;
    END;
GO