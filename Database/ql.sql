CREATE DATABASE QuanLiKaraoke
GO

USE QuanLiKaraoke
GO

--Menu

CREATE TABLE RoomMenu
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Phòng chưa có tên', 
	status1 NVARCHAR(100) NOT NULL DEFAULT N'Trống'
)
GO
CREATE TABLE Account
(
	UserName NVARCHAR(100) NOT NULL PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL,
	Password NVARCHAR(1000) NOT NULL,
	Type1 INT NOT NULL DEFAULT 0

)
GO
CREATE TABLE MenuCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Menu
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL

	FOREIGN KEY (idCategory) REFERENCES dbo.MenuCategory(id)
)
GO 

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATETIME DEFAULT GETDATE(),
	DateCheckOut DATETIME,
	idRoom INT NOT NULL,
	status1 INT NOT NULL DEFAULT 0, 
	discount INT DEFAULT 0

	FOREIGN KEY (idRoom) REFERENCES dbo.RoomMenu(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idMenu INT NOT NULL,
	count1 INT NOT NULL DEFAULT 0
	 
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idMenu) REFERENCES dbo.Menu(id)
)
CREATE TABLE HourMoney
(
	id INT IDENTITY PRIMARY KEY,
	price INT DEFAULT 6000

)

INSERT INTO dbo.Account
	(	UserName ,
		DisplayName ,
		Password ,
		Type1	
	)
VALUES (
		N'NVien',
		N'Nvien1',
		N'1',
		1
	)
INSERT INTO dbo.Account
	(	UserName ,
		DisplayName ,
		Password ,
		Type1	
	)
VALUES (
		N'admin',
		N'admin',
		N'1',
		0
	)
GO

CREATE PROC USP_GetAccountByUserName
@UserName nvarchar(100)
AS 
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

SELECT * FROM dbo.Account WHERE UserName = N'Nvien' AND Password =N'1'


DECLARE @i INT = 1

WHILE @i <= 50 
BEGIN 
	INSERT dbo.RoomMenu(name)VALUES (N'Phòng ' + CAST(@i AS nvarchar(100)))
	SET @i =@i + 1
END

GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.RoomMenu

EXEC dbo.USP_GetTableList
GO
--Menu
INSERT dbo.MenuCategory
	(name)
VALUES (N'Thức ăn')
INSERT dbo.MenuCategory
	(name)
VALUES (N'Thuốc lá')
INSERT dbo.MenuCategory
	(name)
VALUES (N'Bia')
INSERT dbo.MenuCategory
	(name)
VALUES (N'Nước ngọt')

INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'Dĩa trái cây lớn', 1, 150000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'Dĩa trái cây nhỏ', 1, 75000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'Seven', 2, 15000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'555', 2, 35000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'Tiger', 3, 15000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'333', 3, 12000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'Sting', 4, 10000)
INSERT dbo.Menu
	(name, idCategory, price)
VALUES (N'7UP', 4, 10000)

--Bill
INSERT dbo.Bill
	(DateCheckIn, DateCheckOut, idRoom, status1)
VALUES (GETDATE(), NULL, 1, 0)
INSERT dbo.Bill
	(DateCheckIn, DateCheckOut, idRoom, status1)
VALUES (GETDATE(), NULL, 2, 0)

INSERT dbo.BillInfo
	(idBill, idMenu, count1)
VALUES (1, 1, 2)
INSERT dbo.BillInfo
	(idBill, idMenu, count1)
VALUES (2, 3, 4)
INSERT dbo.HourMoney
	(price)
VALUES (60000)

Select * From dbo.Bill
Select * From dbo.BillInfo
Select * From dbo.Menu
Select * From dbo.MenuCategory
ALTER TABLE dbo.Bill ADD totalPrice FLOAT
SELECT * FROM dbo.bill WHERE idRoom = 1

SELECT f.name, bi.count1, f.price, f.price*bi.count1 AS totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Menu AS f WHERE bi.idBill = b.id and bi.idMenu = f.id AND b.status1 = 0 AND b.idRoom = 1

SELECT b.DateCheckIn , b.DateCheckOut ,b.idRoom, DATEDIFF(Minute ,b.DateCheckIn, b.DateCheckOut) as time1, hm.price, (hm.price/60)*DATEDIFF(Minute ,b.DateCheckIn, b.DateCheckOut) as totalPrice FROM dbo.Bill AS b , dbo.HourMoney AS hm WHERE b.idRoom = 1
SELECT * FROM Menu WHERE idCategory = 1
SELECT name , price FROM Menu WHERE idCategory = 1
UPDATE dbo.Bill SET DateCheckOut = GETDATE() WHERE idRoom = 2
SELECT * FROM dbo.bill WHERE idRoom = 4 AND status1 = 0
Select UserName, DisplayName, Password, Type1 FROM dbo.Account
GO

CREATE PROC USP_InsertBill
@idRoom INT
AS 
BEGIN
	INSERT dbo.Bill
			(DateCheckIn,
			DateCheckOut,
			idRoom,
			status1,
			discount	
			)
	VALUES(GETDATE(),
			NULL,
			@idRoom,
			0,
			0)
END
GO

Create PROC USP_InsertBillInfo
@idBill int, @idMenu int, @count1 int
AS
BEGIN
	DECLARE @isExitBillInfo int
	DECLARE @foodCount int = 1
	SELECT @isExitBillInfo = id, @foodCount = count1 FROM BillInfo WHERE idBill = @idBill AND idMenu = @idMenu
	IF(@isExitBillInfo > 0)
	BEGIN
		DECLARE @newCount int = @foodcount + @count1
		IF(@newCount > 0)
	BEGIN
		UPDATE BillInfo SET count1 = @newCount Where id = @isExitBillInfo
	END
		ELSE
	BEGIN
		DELETE BillInfo Where id = @isExitBillInfo 
		END
END
	ELSE
	BEGIN
		IF(@count1 <= 0)
	BEGIN
		return 1;
	END
		ELSE
	 BEGIN
		INSERT INTO BillInfo (idBill,idMenu,count1)
		VALUES(@idBill,@idMenu,@count1)
  END
 END
END
GO
DELETE dbo.BillInfo

DELETE dbo.Bill
GO

Create TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idRoom INT
	
	SELECT @idRoom = idRoom FROM dbo.Bill WHERE id = @idBill AND status1 = 0	
	
	DECLARE @count1 INT
	SELECT @count1 = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBill
	
	IF (@count1 > 0)
	BEGIN
	
		PRINT @idRoom
		PRINT @idBill
		PRINT @count1
		
		UPDATE dbo.RoomMenu SET status1 = N'Có người' WHERE id = @idRoom		
		
	END		
	ELSE
	BEGIN
	PRINT @idRoom
		PRINT @idBill
		PRINT @count1
	UPDATE dbo.RoomMenu SET status1 = N'Trống' WHERE id = @idRoom	
	end
	
END
GO


Create TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idRoom INT
	
	SELECT @idRoom = idRoom FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count1 int = 0
	
	SELECT @count1 = COUNT(*) FROM dbo.Bill WHERE idRoom = @idRoom AND status1 = 0
	
	IF (@count1 = 0)
		UPDATE dbo.RoomMenu SET status1 = N'Trống' WHERE id = @idRoom
END
GO

CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS 
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = Deleted.idBill FROM Deleted
	
	DECLARE @idRoom INT
	SELECT @idRoom = idRoom FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count1 INT = 0
	
	SELECT @count1 = COUNT(*) FROM dbo.BillInfo AS bi, dbo.Bill AS b WHERE b.id = bi.idBill AND b.id = @idBill AND b.status1 = 0
	
	IF (@count1 = 0)
		UPDATE dbo.RoomMenu SET status1 = N'Trống' WHERE id = @idRoom
END
GO

Create PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS 
BEGIN
	SELECT t.name AS [Tên Phòng], DateCheckIn AS [Ngày vào], DateCheckOut AS [Ngày ra], discount AS [Giảm giá] , b.totalPrice AS [Tổng tiền]
	FROM dbo.Bill AS b,dbo.RoomMenu AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status1 = 1
	AND t.id = b.idRoom
END
GO
Create PROC USP_GetListMenu
@idCategory INT
AS
BEGIN
	SELECT m.id AS [STT] , m.name AS [Tên món], m.idCategory AS [Danh mục], m.price AS [Giá Tiền] 
	FROM Menu as m
	WHERE idCategory = @idCategory
END
GO

CREATE PROC USP_SwitchTabel
@idRoom1 INT, @idRoom2 int
AS BEGIN

	DECLARE @idFirstBill int
	DECLARE @idSeconrdBill INT
	
	DECLARE @isFirstRoomEmty INT = 1
	DECLARE @isSecondRoomEmty INT = 1
	
	
	SELECT @idSeconrdBill = id FROM dbo.Bill WHERE idRoom = @idRoom2 AND status1 = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idRoom = @idRoom1 AND status1 = 0
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'
	
	IF (@idFirstBill IS NULL)
	BEGIN
		PRINT '0000001'
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idRoom ,
		          status1
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idRoom1 , -- idTable - int
		          0  -- status - int
		        )
		        
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idRoom = @idRoom1 AND status1 = 0
		
	END
	
	SELECT @isFirstRoomEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'
	
	IF (@idSeconrdBill IS NULL)
	BEGIN
		PRINT '0000002'
		INSERT dbo.Bill
		        ( DateCheckIn ,
		          DateCheckOut ,
		          idRoom ,
		          status1
		        )
		VALUES  ( GETDATE() , -- DateCheckIn - date
		          NULL , -- DateCheckOut - date
		          @idRoom2 , -- idTable - int
		          0  -- status - int
		        )
		SELECT @idSeconrdBill = MAX(id) FROM dbo.Bill WHERE idRoom = @idRoom2 AND status1 = 0
		
	END
	
	SELECT @isSecondRoomEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSeconrdBill
	
	PRINT @idFirstBill
	PRINT @idSeconrdBill
	PRINT '-----------'

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSeconrdBill
	
	UPDATE dbo.BillInfo SET idBill = @idSeconrdBill WHERE idBill = @idFirstBill
	
	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)
	
	DROP TABLE IDBillInfoTable
	
	IF (@isFirstRoomEmty = 0)
		UPDATE dbo.RoomMenu SET status1 = N'Trống' WHERE id = @idRoom2
		
	IF (@isSecondRoomEmty= 0)
		UPDATE dbo.RoomMenu SET status1 = N'Trống' WHERE id = @idRoom1
END
GO

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END