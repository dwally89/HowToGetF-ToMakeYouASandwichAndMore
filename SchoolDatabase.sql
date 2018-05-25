CREATE DATABASE SchoolDatabase
GO
	
USE SchoolDatabase

CREATE TABLE dbo.Classes
(
	Id INT IDENTITY PRIMARY KEY,
	Name VARCHAR(100) NOT NULL 
)
GO

CREATE TABLE dbo.Students
(
	Id INT IDENTITY PRIMARY KEY,
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL 
)
GO

CREATE TABLE dbo.StudentClasses
(
	StudentId INT NOT NULL 
		CONSTRAINT [StudentClasses_dbo.Students_Id_fk]
		REFERENCES dbo.Students (Id),
	ClassId INT NOT null
		CONSTRAINT [StudentClasses_dbo.Classes_Id_fk]
		REFERENCES dbo.Classes
)
GO

