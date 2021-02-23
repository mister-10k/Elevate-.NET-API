CREATE DATABASE Elevate;

GO

USE Elevate;

GO

CREATE TABLE Company (
	ID INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(300) NOT NULL,
	[DisplayName] VARCHAR(500) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT [PK_Company] PRIMARY KEY (ID)
);

GO

CREATE TABLE UserType (
	ID INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(300) NOT NULL,
	[DisplayName] VARCHAR(500) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT [PK_UserType] PRIMARY KEY (ID)
);

GO

CREATE TABLE [User] (
    ID INT IDENTITY (1, 1) NOT NULL,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
	[Email] VARCHAR(300) NOT NULL,
	[Password] VARCHAR(500) NULL,
	UserTypeId  INT NOT NULL,
    CompanyId INT NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
    IsActive BIT NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY (ID),
	CONSTRAINT [U_User_Email] UNIQUE([Email]),
	CONSTRAINT [FK_User_UserTypeId] FOREIGN KEY (UserTypeId) REFERENCES UserType ([Id]),
	CONSTRAINT [FK_User_CompanyId] FOREIGN KEY (CompanyId) REFERENCES Company ([Id])
);

GO

--Create TABLE Employee (
--	ID INT IDENTITY (1, 1) NOT NULL,
--    FirstName VARCHAR(255) NOT NULL,
--    LastName VARCHAR(255) NOT NULL,
--    CompanyId INT NOT NULL,
--	CreatedAt DATETIME NOT NULL,
--	ModifiedAt DATETIME NULL,
--    IsActive BIT NOT NULL,
--	CONSTRAINT [PK_Employee] PRIMARY KEY (ID),
--	CONSTRAINT [FK_Employee_CompanyId] FOREIGN KEY (CompanyId) REFERENCES Company ([Id])
--);

CREATE TABLE Relationship (
	ID INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(500) NOT NULL,
	[DisplayName] VARCHAR(500) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT [PK_Relationship] PRIMARY KEY (ID)
);

GO

CREATE TABLE EmployeeDependent (
	ID INT IDENTITY (1,1) NOT NULL,
	EmployeeId INT NOT NULL,
	FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
	RelationshipId INT NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT [PK_EmployeeDependent] PRIMARY KEY (ID),
	CONSTRAINT [FK_EmployeeDependent_EmployeeId] FOREIGN KEY (EmployeeId) REFERENCES [User] ([Id]),
	CONSTRAINT [FK_EmployeeDependent_RelationshipId] FOREIGN KEY (RelationshipId) REFERENCES Relationship ([Id])
);


