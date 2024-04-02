--Due to nature of creation of test input it's advised to use it without adding constraints to [Table] and [Owner] tables due to possibility of duplicates
--Some of input stored procedures are specificaly created for working with [sp_Test_Input]

CREATE OR ALTER PROCEDURE [dbo].[sp_Bill_Test_Insert]
@TenantId [uniqueidentifier],
@AssetId [uniqueidentifier],
@Amount [numeric](18,2),
@EndDate [date],
@IssueDate [date]
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
		    FROM [dbo].[Asset] [Asset]
			WHERE [Asset].[AssetId] = @AssetId
		)
		BEGIN
            RAISERROR( 'There is no such asset', 11, 5) WITH NOWAIT;
		END;
			
		DECLARE @BillId [uniqueidentifier] = NEWID();
		INSERT INTO [dbo].[Bill] ([BillId], [TenantId], [AssetId], [BillAmount], [IssueDate], [EndDate])
		SELECT @BillId, @TenantId, @AssetId, @Amount, @IssueDate, @EndDate;
		SELECT @BillId AS [BillId];
    END TRY
    BEGIN CATCH
		    DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 5);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Impost_Test_Insert]
@Tax [decimal](4,2),
@Fine [decimal](3,2),
@PaymentDay [int],
@StartDay [datetime2],
@EndDay [datetime2]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
	    
		IF EXISTS(
            SELECT 1
            FROM [dbo].[Impost] [Impost]
            WHERE (@StartDay BETWEEN [Impost].[StartDate] AND [Impost].[EndDate])
			    OR @EndDay BETWEEN [Impost].[StartDate] AND [Impost].[EndDate]
		)
		BEGIN
		    RAISERROR( 'There already is imposts at this time', 11, 6) WITH NOWAIT;
		END;

		BEGIN TRANSACTION;
			INSERT INTO [dbo].[Impost] ([ImpostId], [Tax], [Fine], [PaymentDay], [StartDate], [EndDate])
			SELECT NEWID(), @Tax, @Fine, @PaymentDay, @StartDay, @EndDay;
		COMMIT TRANSACTION;
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

CREATE OR ALTER PROCEDURE [dbo].[sp_Price_Test_Insert]
@StartDate [datetime2],
@Value [numeric](18,2),
@EndDate [datetime2],
@RoomTypeId [int]
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
		IF EXISTS(SELECT 1
		    FROM [dbo].[Price] [Price]
			WHERE [Price].[RoomTypeId] = @RoomTypeId AND @StartDate BETWEEN [Price].[StartDate] AND [Price].[EndDate]
		)
		BEGIN
		    RAISERROR( 'There is already price in this period for given room type' , 11, 9) WITH NOWAIT;
		END;
		
		IF NOT EXISTS(SELECT 1
			FROM [dbo].[RoomType]
			WHERE [RoomTypeId] = @RoomTypeId
		)
		BEGIN
			RAISERROR( 'There is no such room type' , 11, 9) WITH NOWAIT;
		END;

		BEGIN TRANSACTION;
		    INSERT INTO [dbo].[Price] ([PriceId], [StartDate], [Value], [EndDate], [RoomTypeId])
		    SELECT NEWID(), @StartDate, @Value, @EndDate, @RoomTypeId;
		COMMIT TRANSACTION;
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

CREATE OR ALTER PROCEDURE [dbo].[sp_Payment_Test_Insert]
@TenantId [uniqueidentifier],
@BillId [uniqueidentifier],
@PaymentDay [datetime2],
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

		INSERT INTO [dbo].[Payment] ([PaymentId], [TenantId], [BillId], [PaymentDay], [Amount])
		SELECT NEWID(), @TenantId, @BillId, @PaymentDay, @Amount;

    END TRY
    BEGIN CATCH
		    DECLARE @Message [nvarchar](100) = 'An error occurred: ' + ERROR_MESSAGE();
        RAISERROR( @Message , 11, 8);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_Test_Input]
AS
BEGIN
	SET NOCOUNT ON;
    --Temp tables
	CREATE TABLE #TempAddressCity
	([City] [nvarchar](255));

	CREATE TABLE #TempAddressStreet
	([Street] [nvarchar](255));

	CREATE TABLE #TempAddressBuilding
	([Building] [nvarchar](255));

	CREATE TABLE #TempAddressId
    ([AddressId] [uniqueidentifier]);
    
	CREATE TABLE #TempAddressData
    ([City] [nvarchar](255),
	[Street] [nvarchar](255),
	[Building] [nvarchar](255));

	CREATE TABLE #TempFirstName
	([FirstName] [nvarchar](20));

	CREATE TABLE #TempLastName
	([LastName] [nvarchar](20));

	CREATE TABLE #TempOwnerId
	([OwnerId] [uniqueidentifier]);

	CREATE TABLE #TempOwnerData
    ([Name] [nvarchar](50));

	CREATE TABLE #TempOwner
	([Name] [nvarchar](50),
	[AddressId] [uniqueidentifier]);

	CREATE TABLE #TempTenantName
	([Name] [nvarchar](50));

	CREATE TABLE #TempTenantDirectorName
	([Director] [nvarchar](50));

	CREATE TABLE #TempTenantBankName
	([BankName] [nvarchar](50));

	CREATE TABLE #TempTenantDescription
	([Description] [nvarchar](50));

	CREATE TABLE #TempTenantId
	([TenantId] [uniqueidentifier])

	CREATE TABLE #TempTenantData
	([Name] [nvarchar](50),
	[BankName] [nvarchar](50),
	[Director] [nvarchar](50),
	[Description] [nvarchar](255));

	CREATE TABLE #TempTenant
    ([Name] [nvarchar](50),
	[BankName] [nvarchar](50),
	[AddressId] [uniqueidentifier],
	[Director] [nvarchar](50),
	[Description] [nvarchar](255));

	CREATE TABLE #TempRoomTypeId
	([RoomTypeId] [int]);

	CREATE TABLE #TempRoomTypeData
	([Name] [nvarchar](50));

	CREATE TABLE #TempDate
	([StartDate] [datetime2],
	[EndDate] [datetime2],);

	CREATE TABLE #TempPriceValue
	([Value] [numeric](18,2));

	CREATE TABLE #TempPriceData
	([StartDate] [datetime2],
	[Value] [numeric](18,2),
	[EndDate] [datetime2]);

	CREATE TABLE #TempPrice
    ([StartDate] [datetime2],
	[Value] [numeric](18,2),
	[EndDate] [datetime2],
	[RoomTypeId] [int]);

	CREATE TABLE #TempRoomId
	([RoomId] [uniqueidentifier]);

	CREATE TABLE #TempRoomData
	([Number] [int],
	[Area] [numeric](18,2));

	CREATE TABLE #TempRoom
    ([Number] [int],
	[Area] [numeric](18,2),
	[RoomTypeId] [int]);

	CREATE TABLE #TempAssetId
	([AssetId] [uniqueidentifier]);

	CREATE TABLE #TempAsset
    ([OwnerId] [uniqueidentifier],
	[RoomId] [uniqueidentifier]);

	CREATE TABLE #TempRentId
	([RentId] [uniqueidentifier]);

	CREATE TABLE #TempRent
    ([AssetId] [uniqueidentifier],
	[TenantId] [uniqueidentifier],
	[StartDate] [datetime2],
	[EndDate] [datetime2]);

	CREATE TABLE #TempBillId
	([BillId] [uniqueidentifier]);

	CREATE TABLE #TempBillData
	([BillAmount] [numeric](18,2),
	[IssueDate] [date],
	[EndDate] [date]);

	CREATE TABLE #TempBill
    ([BillAmount] [numeric](18,2),
	[AssetId] [uniqueidentifier],
	[TenantId] [uniqueidentifier],
	[IssueDate] [date],
	[EndDate] [date]);

	CREATE TABLE #TempPaymentData
	([PaymentDay] [datetime2],
	[Amount] [numeric](18,2));

	CREATE TABLE #TempPayment
    ([TenantId] [uniqueidentifier],
	[BillId] [uniqueidentifier],
	[PaymentDay] [datetime2],
	[Amount] [numeric](18,2));

	CREATE TABLE #TempImpostData
	([Tax] [numeric](18,2),
	[Fine] [numeric](18,2),
	[PaymentDay] [int],
	[StartDay] [datetime2],
	[EndDay] [datetime2]);

	--Variables
	DECLARE
	    @City [nvarchar](255), 
		@Street [nvarchar](255), 
		@Building [nvarchar](255),
		@Name [nvarchar](50), 
		@AddressId [uniqueidentifier],
		@BankName [nvarchar](50), 
		@Director [nvarchar](50), 
		@Description [nvarchar](255),
		@Value [numeric](18,2), 
		@EndDate [datetime2], 
		@RoomTypeId [int],
		@Number [int], 
		@Area [numeric](18,2),
		@OwnerId [uniqueidentifier], 
		@RoomId[uniqueidentifier],
		@AssetId [uniqueidentifier], 
		@TenantId [uniqueidentifier],
		@BillAmount [numeric](18,2), 
		@IssueDate [date],
		@Tax [numeric](4,2), 
		@Fine [numeric](3,2), 
		@PaymentDays[int], 
		@StartDay [datetime2], 
		@EndDay [datetime2],
		@BillId [uniqueidentifier],
		@PaymentDay [datetime2], 
		@Amount [numeric](18,2),
		@AmountOfRoomTypes [int],
		@StartDate [datetime2];

	--Cursors
    DECLARE Address_cursor CURSOR
    FOR SELECT [City], [Street], [Building]
    FROM #TempAddressData;

	DECLARE Owner_cursor CURSOR
    FOR SELECT [Name], [AddressId]
    FROM #TempOwner;

	DECLARE Tenant_cursor CURSOR
    FOR SELECT [Name], [BankName], [AddressId], [Director], [Description]
    FROM #TempTenant;

	DECLARE RoomType_cursor CURSOR
    FOR SELECT [Name]
    FROM #TempRoomTypeData;

	DECLARE Price_cursor CURSOR
    FOR SELECT [StartDate], [Value], [EndDate], [RoomTypeId]
    FROM #TempPrice;

	DECLARE Room_cursor CURSOR
    FOR SELECT [Number], [Area], [RoomTypeId]
    FROM #TempRoom;

	DECLARE Asset_cursor CURSOR
    FOR SELECT [OwnerId], [RoomId]
    FROM #TempAsset;

	DECLARE Rent_cursor CURSOR
    FOR SELECT [AssetId], [TenantId], [StartDate], [EndDate]
    FROM #TempRent;

	DECLARE Bill_cursor CURSOR
    FOR SELECT [BillAmount], [AssetId], [TenantId], [IssueDate], [EndDate]
    FROM #TempBill;

	DECLARE Payment_cursor CURSOR
    FOR SELECT [TenantId], [BillId], [PaymentDay], [Amount]
    FROM #TempPayment;

	DECLARE Impost_cursor CURSOR
    FOR SELECT [Tax], [Fine], [PaymentDay], [StartDay], [EndDay]
    FROM #TempImpostData;

	--User table insert
	IF NOT EXISTS(SELECT 1 FROM [dbo].[User] WHERE [Name] = 'test_user')
	INSERT INTO [dbo].[User]([UserId],[Name], [Password])
	SELECT NEWID(), 'test_user', 'temp_password';

	--Temp table inserts
	INSERT INTO #TempAddressCity VALUES 
	('New York'),
	('Los Angeles'),
	('Chicago'),
	('Houston'),
	('Phoenix'),
	('Philadelphia'),
	('San Antonio'),
	('San Diego'),
	('Dallas'),
	('San Jose'),
	('Austin'),
	('Jacksonville'),
	('San Francisco'),
	('Indianapolis'),
	('Columbus'),
	('Fort Worth'),
	('Charlotte'),
	('Seattle'),
	('Denver'),
	('El Paso'),
	('Detroit'),
	('Washington'),
	('Boston'),
	('Memphis'),
	('Nashville'),
	('Portland'),
	('Oklahoma City'),
	('Las Vegas'),
	('Baltimore'),
	('Louisville'),
	('Milwaukee'),
	('Albuquerque'),
	('Tucson'),
	('Fresno'),
	('Sacramento'),
	('Kansas City'),
	('Long Beach'),
	('Mesa'),
	('Atlanta'),
	('Colorado Springs'),
	('Miami'),
	('Raleigh'),
	('Omaha'),
	('Virginia Beach'),
	('Oakland'),
	('Minneapolis'),
	('Tulsa'),
	('Wichita'),
	('Arlington'),
	('New Orleans');

	INSERT INTO #TempAddressStreet VALUES 
	('Main Street'),
	('First Street'),
	('Second Street'),
	('Third Street'),
	('Fourth Street'),
	('Fifth Avenue'),
	('Elm Street'),
	('Maple Avenue'),
	('Oak Street'),
	('Park Avenue'),
	('Washington Street'),
	('Broadway'),
	('High Street'),
	('Market Street'),
	('Church Street'),
	('Chestnut Street'),
	('Pine Street'),
	('Center Street'),
	('Spruce Street'),
	('School Street'),
	('Bridge Street'),
	('Water Street'),
	('North Street'),
	('South Street'),
	('West Street'),
	('East Street'),
	('Union Street'),
	('Spring Street'),
	('Madison Avenue'),
	('Franklin Street'),
	('Hill Street'),
	('Front Street'),
	('Liberty Street'),
	('Jackson Street'),
	('Victoria Street'),
	('Sunset Boulevard'),
	('Lake Street'),
	('Grove Street'),
	('Mill Street'),
	('River Road'),
	('Park Street'),
	('Lincoln Street'),
	('Smith Street'),
	('Grant Avenue'),
	('Adams Street'),
	('Pearl Street'),
	('Jefferson Street'),
	('State Street'),
	('Court Street'),
	('Willow Street');

	INSERT INTO #TempAddressBuilding VALUES 
	('123'),
	('456'),
	('789'),
	('101'),
	('202'),
	('303'),
	('404'),
	('505'),
	('606'),
	('707'),
	('808'),
	('909'),
	('111'),
	('222'),
	('333'),
	('444'),
	('555'),
	('666'),
	('777'),
	('888'),
	('999'),
	('121'),
	('232'),
	('343'),
	('454'),
	('565'),
	('676'),
	('787'),
	('898'),
	('919'),
	('131'),
	('242'),
	('353'),
	('464'),
	('575'),
	('686'),
	('797'),
	('808'),
	('929'),
	('141'),
	('252'),
	('363'),
	('474'),
	('585'),
	('696');

	INSERT INTO #TempAddressData
	SELECT TOP(20) [AddressCity].[City], [AddressStreet].[Street], [AddressBuilding].[Building]
	FROM [#TempAddressCity] [AddressCity], [#TempAddressStreet] [AddressStreet], [#TempAddressBuilding] [AddressBuilding]
	ORDER BY NEWID();

	INSERT INTO #TempFirstName VALUES 
    ('John'),
	('Jane'),
	('Michael'),
	('Emily'),
	('David'),
	('Sarah'),
	('Chris'),
	('Laura'),
	('Matthew'),
	('Jennifer'),
	('Robert'),
	('Amanda'),
	('Daniel'),
	('Stephanie'),
	('James'),
	('Jessica'),
	('Andrew'),
	('Elizabeth'),
	('Brian'),
	('Megan'),
	('William'),
	('Nicole'),
	('Kevin'),
	('Samantha'),
	('Thomas'),
	('Ashley'),
	('Ryan'),
	('Melissa'),
	('Steven'),
	('Lauren'),
	('Joseph'),
	('Christina'),
	('Jason'),
	('Amy'),
	('Patrick'),
	('Michelle'),
	('Timothy'),
	('Kimberly'),
	('Richard'),
	('Rachel'),
	('Alex'),
	('Victoria'),
	('Charles'),
	('Hannah'),
	('Peter'),
	('Olivia'),
	('Jonathan'),
	('Sophia'),
	('Ethan'),
	('Madison'),
	('Benjamin'),
	('Emma'),
	('Gabriel'),
	('Grace'),
	('Samuel'),
	('Chloe'),
	('Tyler'),
	('Lily'),
	('Nicholas'),
	('Ava');

	INSERT INTO #TempLastName VALUES 
	('Smith'),
	('Johnson'),
	('Williams'),
	('Jones'),
	('Brown'),
	('Davis'),
	('Miller'),
	('Wilson'),
	('Moore'),
	('Taylor'),
	('Anderson'),
	('Thomas'),
	('Jackson'),
	('White'),
	('Harris'),
	('Martin'),
	('Thompson'),
	('Garcia'),
	('Martinez'),
	('Robinson'),
	('Clark'),
	('Rodriguez'),
	('Lewis'),
	('Lee'),
	('Walker'),
	('Hall'),
	('Allen'),
	('Young'),
	('Hernandez'),
	('King'),
	('Wright'),
	('Lopez'),
	('Hill'),
	('Scott'),
	('Green'),
	('Adams'),
	('Baker'),
	('Gonzalez'),
	('Nelson'),
	('Carter');

	INSERT INTO #TempOwnerData
	SELECT TOP(10) CONCAT ([FirstName].[FirstName], [LastName].[LastName]) AS [Name]
	FROM [#TempFirstName] [FirstName], [#TempLastName] [LastName]
	ORDER BY NEWID();

	INSERT INTO #TempTenantName VALUES 
	('Acme Corporation'),
	('Widget Industries'),
	('Global Logistics Inc.'),
	('Tech Solutions Ltd.'),
	('Innovative Ventures LLC'),
	('Pioneer Enterprises'),
	('Summit Holdings'),
	('Apex Innovations'),
	('Prime Systems'),
	('Dynamic Enterprises'),
	('Evergreen Group'),
	('Omega Solutions'),
	('Silverline Technologies'),
	('Unified Services'),
	('Frontier Industries'),
	('Golden Gate Enterprises'),
	('Northern Lights Ventures'),
	('Titanium Solutions'),
	('Strategic Partners Inc.'),
	('Bluebird Enterprises'),
	('Vanguard Holdings'),
	('Matrix Corporation'),
	('Phoenix Enterprises'),
	('Crimson Solutions'),
	('Arrowhead Ventures'),
	('Redwood Enterprises'),
	('Sunrise Technologies'),
	('Magnolia Group'),
	('Azure Innovations'),
	('Pacific Coast Ventures');

	INSERT INTO #TempTenantDirectorName
	SELECT TOP(10) CONCAT ([FirstName].[FirstName], ' ' , [LastName].[LastName]) AS [Name]
	FROM [#TempFirstName] [FirstName], [#TempLastName] [LastName]
	ORDER BY NEWID();

	INSERT INTO #TempTenantBankName VALUES 
	('City Bank'),
	('First National Bank'),
	('Bank of America'),
	('Wells Fargo'),
	('Chase Bank'),
	('Citibank'),
	('HSBC Bank'),
	('TD Bank'),
	('PNC Bank'),
	('US Bank'),
	('Capital One Bank'),
	('BB&T Bank'),
	('SunTrust Bank'),
	('Regions Bank'),
	('Santander Bank'),
	('KeyBank'),
	('Fifth Third Bank'),
	('M&T Bank'),
	('Huntington Bank'),
	('Citizens Bank');

	INSERT INTO #TempTenantDescription VALUES 
	('Leading provider of IT solutions'),
	('Innovative technology company driving change'),
	('Manufacturer of high-quality widgets'),
	('Global leader in financial services'),
	('Offering cutting-edge software solutions'),
	('Providing top-notch customer service'),
	('Industry pioneer in renewable energy'),
	('Leader in healthcare services'),
	('Expert in marketing and advertising'),
	('Delivering excellence in consulting services'),
	('Leading the way in sustainable practices'),
	('Provider of premium security solutions'),
	('Bringing creativity to design and branding'),
	('Specializing in real estate development'),
	('Supplier of industrial equipment'),
	('Focused on improving education worldwide'),
	('Manufacturer of consumer electronics'),
	('Offering comprehensive insurance services'),
	('Leader in telecommunications'),
	('Specializing in food and beverage distribution'),
	('Global player in automotive manufacturing'),
	('Innovator in pharmaceuticals'),
	('Leader in hospitality and tourism'),
	('Expert in financial consulting'),
	('Provider of legal services'),
	('Leading the market in entertainment'),
	('Specializing in aerospace engineering'),
	('Global leader in fashion retail'),
	('Innovative leader in biotechnology');

	INSERT TOP(20) INTO #TempTenantData
	SELECT [TenantName].[Name], [BankName].[BankName], [DirectorName].[Director], [Description].[Description]
	FROM [#TempTenantName] [TenantName], [#TempTenantDirectorName] [DirectorName], [#TempTenantBankName] [BankName], [#TempTenantDescription] [Description]
	ORDER BY NEWID();

	INSERT INTO #TempRoomTypeData VALUES 
	('Single Room'),
	('Double Room'),
	('Suite'),
	('Studio Apartment'),
	('Deluxe Room'),
	('Executive Suite'),
	('Family Room'),
	('Penthouse Suite'),
	('Duplex Apartment'),
	('Villa');

	DECLARE @start [datetime2] = '2022-01-01';
	DECLARE @end [datetime2] = GETDATE();

	WITH DateCTE AS (
		SELECT @start AS StartDate, DATEADD(MONTH, 1, @start) AS EndDate
		UNION ALL
		SELECT DATEADD(MONTH, 1, StartDate), DATEADD(MONTH, 2, StartDate)
		FROM DateCTE
		WHERE DATEADD(MONTH, 1, StartDate) < @end
	)
	INSERT INTO #TempDate ([StartDate], [EndDate])
	SELECT StartDate, DATEADD(DAY, -1, EndDate) AS EndDate
	FROM DateCTE;

	INSERT INTO #TempPriceValue
	SELECT TOP 20 CONVERT([numeric](18,2),(ABS(CHECKSUM(NewId())) + 500) % 2000 + 200) AS [Value]
    FROM
        [sys].[all_columns];

	INSERT INTO #TempPriceData
	SELECT [PriceDate].[StartDate], [PriceValue].[Value], [PriceDate].[EndDate]
	FROM [#TempDate] [PriceDate], [#TempPriceValue] [PriceValue]
	ORDER BY NEWID();

	INSERT INTO #TempRoomData
    SELECT TOP 10
	    CONVERT([int],(ABS(CHECKSUM(NewId()))) % 1000 + 10) AS [Number],
		CONVERT([numeric](18,2),(ABS(CHECKSUM(NewId()))) % 100 + 50) AS [Area]
	FROM
	    [sys].[all_columns];

	INSERT INTO #TempImpostData
	SELECT CONVERT([numeric](18,2),(ABS(CHECKSUM(NewId()))) % 1 + 0.2) AS [Tax], CONVERT([numeric](18,2),(ABS(CHECKSUM(NewId()))) % 0.1 + 0.02) AS [Fine], CONVERT([int],(ABS(CHECKSUM(NewId()))) % 15 + 1) AS [PaymentDay], [TempDate].[StartDate], [TempDate].[EndDate] 
	FROM [#TempDate] [TempDate];

	--Address
	OPEN Address_cursor;

	FETCH NEXT FROM Address_cursor INTO 
    @City, @Street, @Building

	WHILE @@FETCH_STATUS = 0
	BEGIN
	   
	    INSERT INTO #TempAddressId EXEC [dbo].[sp_Address_Insert] @City = @City, @Street = @Street, @Building = @Building

	    FETCH NEXT FROM Address_cursor INTO 
        @City, @Street, @Building
	END

	CLOSE Address_cursor;

    --Owner
    INSERT INTO #TempOwner
	SELECT TOP 5 [OwnerData].[Name], [AddressId].[AddressId]
	FROM [#TempOwnerData] [OwnerData], [#TempAddressId] [AddressId]
	ORDER BY NEWID();

	OPEN Owner_cursor;

	FETCH NEXT FROM Owner_cursor INTO 
    @Name, @AddressId

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempOwnerId EXEC [dbo].[sp_Owner_Insert] @Name = @Name, @AddressId = @AddressId

	    FETCH NEXT FROM Owner_cursor INTO 
        @Name, @AddressId
	END

	CLOSE Owner_cursor;

	--Tenant
    INSERT INTO #TempTenant
	SELECT TOP 10 [combine].[Name], [combine].[BankName], [combine].[AddressId], [combine].[Director], [combine].[Description]
	FROM(SELECT [t1].[Name], [t1].[BankName], [t1].[Director], [t1].[Description], [t2].[AddressId]
        FROM #TempTenantData AS [t1]
        CROSS JOIN #TempAddressId AS [t2]) AS [combine]
	ORDER BY NEWID();
   
	OPEN Tenant_cursor;

	FETCH NEXT FROM Tenant_cursor INTO 
    @Name, @BankName, @AddressId, @Director, @Description;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempTenantId EXEC [dbo].[sp_Tenant_Insert] @AddressId = @AddressId, @Name = @Name, @BankName = @BankName, @Director = @Director, @Description = @Description;

	    FETCH NEXT FROM Tenant_cursor INTO 
    @Name, @BankName, @AddressId, @Director, @Description;
	END

	CLOSE Tenant_cursor;

	--RoomType
	OPEN RoomType_cursor;

	FETCH NEXT FROM RoomType_cursor INTO 
    @Name;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempRoomTypeId EXEC [dbo].[sp_RoomType_Insert] @Name = @Name;
	    FETCH NEXT FROM RoomType_cursor INTO 
        @Name;
	END
	CLOSE RoomType_cursor;

	--Price
    INSERT INTO #TempPrice
	SELECT [combine].[StartDate], [combine].[Value], [combine].[EndDate], [combine].[RoomTypeId]
	FROM(SELECT [t1].[StartDate], [t1].[Value], [t1].[EndDate], [t2].[RoomTypeId]
        FROM #TempPriceData AS [t1], #TempRoomTypeId AS [t2]) AS [combine]

	OPEN Price_cursor;

	FETCH NEXT FROM Price_cursor INTO 
    @StartDate, @Value, @EndDate, @RoomTypeId;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    EXEC [dbo].[sp_Price_Test_Insert] @StartDate = @StartDate, @Value = @Value, @EndDate = @EndDate, @RoomTypeId = @RoomTypeId;

	    FETCH NEXT FROM Price_cursor INTO 
		@StartDate, @Value, @EndDate, @RoomTypeId;
	END

	CLOSE Price_cursor;

	--Room
    INSERT INTO #TempRoom
	SELECT TOP 10 [combine].[Number], [combine].[Area], [combine].[RoomTypeId]
	FROM(SELECT [t1].[Number], [t1].[Area], [t2].[RoomTypeId]
        FROM #TempRoomData AS [t1]
        CROSS JOIN #TempRoomTypeId AS [t2]) AS [combine]
	ORDER BY NEWID();
   
	OPEN Room_cursor;

	FETCH NEXT FROM Room_cursor INTO 
    @Number, @Area, @RoomTypeId

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempRoomId EXEC [dbo].[sp_Room_Insert] @Number = @Number, @Area = @Area, @RoomTypeId = @RoomTypeId;

	    FETCH NEXT FROM Room_cursor INTO 
        @Number, @Area, @RoomTypeId
	END

	CLOSE Room_cursor;

	--Asset
    INSERT INTO #TempAsset
	SELECT [combine].[OwnerId], [combine].[RoomId]
	FROM(SELECT [t1].[OwnerId], [t2].[RoomId]
        FROM #TempOwnerId AS [t1]
        CROSS JOIN #TempRoomId AS [t2]) AS [combine]

	OPEN Asset_cursor;

	FETCH NEXT FROM Asset_cursor INTO 
    @OwnerId, @RoomId

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempAssetId EXEC [dbo].[sp_Asset_Insert] @OwnerId = @OwnerId, @RoomId = @RoomId;

	    FETCH NEXT FROM Asset_cursor INTO 
        @OwnerId, @RoomId
	END

	CLOSE Asset_cursor;

	--Rent
    INSERT INTO #TempRent
	SELECT [combine].[AssetId], [combine].[TenantId], [combine].[StartDate], [combine].[EndDate]
	FROM(SELECT [t1].[AssetId], [t2].[TenantId], [t3].[StartDate], [t3].[EndDate]
        FROM #TempAssetId AS [t1], #TempTenantId AS [t2], #TempDate AS [t3]) AS [combine]
	ORDER BY NEWID() DESC;

	OPEN Rent_cursor;

	FETCH NEXT FROM Rent_cursor INTO 
    @AssetId, @TenantId, @StartDate, @EndDate

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempRentId EXEC [dbo].[sp_Rent_Insert] @AssetId = @AssetId, @TenantId = @TenantId, @StartDate = @StartDate, @EndDate = @EndDate;

	    FETCH NEXT FROM Rent_cursor INTO 
		@AssetId, @TenantId, @StartDate, @EndDate
	END

	CLOSE Rent_cursor;

	--Bill
	INSERT INTO #TempBill
	SELECT [Price].[Value] * [Room].[Area] AS [BillAmount], 
		[Asset].[AssetId], 
		[Rent].[TenantId], 
		[Rent].[EndDate] AS [IssueDate],  
		CASE
			WHEN DATEADD(MONTH, 1, [Rent].[EndDate]) < GETDATE() THEN DATEADD(MONTH, 1, [Rent].[EndDate])
			ELSE NULL
		END AS [EndDate]
	FROM [#TempRentId] [TempRentId]
	LEFT JOIN [dbo].[Rent] AS [Rent] ON [Rent].[RentId] = [TempRentId].[RentId]
	LEFT JOIN [dbo].[Asset] AS [Asset] ON [Asset].[AssetId] = [Rent].[AssetId]
	LEFT JOIN [dbo].[Room] AS [Room] ON [Room].[RoomId] = [Asset].[RoomId]
	LEFT JOIN [dbo].[Price] AS [Price] ON [Price].[RoomTypeId] = [Room].[RoomTypeId]
	WHERE [Rent].[EndDate] BETWEEN [Price].[StartDate] AND [Price].[EndDate];

	OPEN Bill_cursor;

	FETCH NEXT FROM Bill_cursor INTO 
    @BillAmount, @AssetId, @TenantId, @IssueDate, @EndDate;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    INSERT INTO #TempBillId EXEC [dbo].[sp_Bill_Test_Insert] @Amount = @BillAmount, @IssueDate = @IssueDate, @EndDate = @EndDate, @AssetId = @AssetId, @TenantId = @TenantId;

	   FETCH NEXT FROM Bill_cursor INTO 
       @BillAmount, @AssetId, @TenantId, @IssueDate, @EndDate;
	END

	CLOSE Bill_cursor;

	--Payment
    INSERT INTO #TempPayment
	SELECT [combine].[TenantId], [combine].[BillId], [combine].[PaymentDay], [combine].[Amount]
	FROM(SELECT [t2].[TenantId], [t1].[BillId], [t2].[EndDate] AS [PaymentDay], [t2].[BillAmount] AS [Amount]
        FROM #TempBillId AS [t1]
		LEFT JOIN [dbo].[Bill] AS [t2] ON [t2].[BillId] = [t1].[BillId]) AS [combine]
	ORDER BY NEWID();

	OPEN Payment_cursor;

	FETCH NEXT FROM Payment_cursor INTO 
    @TenantId, @BillId, @PaymentDay, @Amount;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    EXEC [dbo].[sp_Payment_Test_Insert] @TenantId = @TenantId, @BillId = @BillId, @PaymentDay = @PaymentDay, @Amount = @Amount;

	    FETCH NEXT FROM Payment_cursor INTO 
        @TenantId, @BillId, @PaymentDay, @Amount;
	END

	CLOSE Payment_cursor;

	--Impost
	OPEN Impost_cursor;

	FETCH NEXT FROM Impost_cursor INTO 
    @Tax, @Fine, @PaymentDays, @StartDay, @EndDay;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	    EXEC [dbo].[sp_Impost_Test_Insert] @Tax = @Tax, @Fine = @Fine, @PaymentDay = @PaymentDays, @StartDay = @StartDay, @EndDay = @EndDay;

	    FETCH NEXT FROM Impost_cursor INTO 
        @Tax, @Fine, @PaymentDays, @StartDay, @EndDay;
	END

	CLOSE Impost_cursor;

	--Temp tables deletion
	DROP TABLE #TempAddressCity;
	DROP TABLE #TempAddressStreet;
	DROP TABLE #TempAddressBuilding;
	DROP TABLE #TempAddressId;
	DROP TABLE #TempAddressData;
	DROP TABLE #TempFirstName;
	DROP TABLE #TempLastName;
	DROP TABLE #TempOwnerId;
	DROP TABLE #TempOwnerData;
	DROP TABLE #TempOwner;
	DROP TABLE #TempTenantName;
	DROP TABLE #TempTenantDirectorName;
	DROP TABLE #TempTenantBankName;
	DROP TABLE #TempTenantDescription;
	DROP TABLE #TempTenantId;
	DROP TABLE #TempTenantData;
	DROP TABLE #TempTenant;
	DROP TABLE #TempRoomTypeId;
	DROP TABLE #TempRoomTypeData;
	DROP TABLE #TempDate;
	DROP TABLE #TempPriceValue;
	DROP TABLE #TempPriceData;
	DROP TABLE #TempPrice;
	DROP TABLE #TempRoomId;
	DROP TABLE #TempRoomData;
	DROP TABLE #TempRoom;
	DROP TABLE #TempAssetId;
	DROP TABLE #TempAsset;
	DROP TABLE #TempRentId;
	DROP TABLE #TempRent;
	DROP TABLE #TempBillId;
	DROP TABLE #TempBillData;
	DROP TABLE #TempBill;
	DROP TABLE #TempPaymentData;
	DROP TABLE #TempPayment;
	DROP TABLE #TempImpostData;
END;
GO