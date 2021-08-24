DROP DATABASE IF EXISTS [azure_functions_test]
GO

CREATE DATABASE [azure_functions_test]
GO

USE [azure_functions_test]
GO

DROP TABLE IF EXISTS [dbo].[todos]
GO

CREATE TABLE [dbo].[todos] (
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [TaskDescription] NVARCHAR(50) NOT NULL, 
    [IsCompleted] BIT NOT NULL, 
    [CreatedTime] DATETIME NOT NULL
)
GO