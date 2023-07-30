USE tempdb;
GO
DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = N'APIIntegrationDb')
BEGIN
    SET @SQL = N'USE [APIIntegrationDb];

                 ALTER DATABASE APIIntegrationDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE [tempdb];

                 DROP DATABASE APIIntegrationDb;';
    EXEC (@SQL);
END

GO
CREATE DATABASE APIIntegrationDb;
GO
USE APIIntegrationDb;
GO
CREATE TABLE dbo.[Task] (
    [Id] INT PRIMARY KEY IDENTITY (1, 1),
    [Name] NVARCHAR (50) NOT NULL,
	[Date] DateTime NOT NULL,
    [Type] NVARCHAR (50) NOT NULL,
    [IsProcessed] BIT NOT NULL DEFAULT(0),
	[ModifyDate] DateTime NULL,
	[ModifyBy] NVARCHAR (50) NULL
);
GO
INSERT INTO dbo.[Task] ([Name], [Date], [Type], [IsProcessed], [ModifyDate], [ModifyBy])
VALUES ('Task1', GETDATE(), 'Init', 0, null, null),
	   ('Task2', GETDATE(), 'Update', 1, GETDATE(), 'user'),
	   ('Task3', GETDATE(), 'Reject', 0, null, null),
	   ('Task4', GETDATE(), 'Confirm', 0, null, null),
	   ('Task5', GETDATE(), 'Reject', 1, GETDATE(), 'user'),
	   ('Task6', GETDATE(), 'Confirm', 0, null, null),
	   ('Task7', GETDATE(), 'Init', 1, GETDATE(), 'user'),
	   ('Task8', GETDATE(), 'Update', 0, null, null);
GO