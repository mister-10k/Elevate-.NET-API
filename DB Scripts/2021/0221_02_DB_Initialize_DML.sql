
USE Elevate;

GO

IF NOT EXISTS(SELECT TOP 1 1 FROM [Company] where [Name]='Paylocity')
INSERT INTO Company ([Name],DisplayName,CreatedAt,IsActive) VALUES ('Paylocity', 'Paylocity', GETDATE(), 1)
IF NOT EXISTS(SELECT TOP 1 1 FROM [Company] where [Name]='Walmart')
INSERT INTO Company ([Name],DisplayName,CreatedAt,IsActive) VALUES ('Walmart', 'Walmart', GETDATE(), 1)
IF NOT EXISTS(SELECT TOP 1 1 FROM [Company] where [Name]='Amazon')
INSERT INTO Company ([Name],DisplayName,CreatedAt,IsActive) VALUES ('Amazon', 'Amazon', GETDATE(), 1)
IF NOT EXISTS(SELECT TOP 1 1 FROM [Company] where [Name]='Disney')
INSERT INTO Company ([Name],DisplayName,CreatedAt,IsActive) VALUES ('Disney', 'Disney', GETDATE(), 1)

GO

IF NOT EXISTS(SELECT TOP 1 1 FROM UserType where [Name]='BenifitsManager')
INSERT INTO UserType ([Name],DisplayName,CreatedAt,IsActive) VALUES ('BenifitsManager', 'Benifits Manager', GETDATE(), 1)
IF NOT EXISTS(SELECT TOP 1 1 FROM UserType where [Name]='Employee')
INSERT INTO UserType ([Name],DisplayName,CreatedAt,IsActive) VALUES ('Employee', 'Employee', GETDATE(), 1)

GO

DECLARE @UserTypeId INT = (SELECT ID FROM UserType WHERE Name='BenifitsManager');
DECLARE @CompanyId INT = (SELECT ID FROM Company WHERE Name='Paylocity')

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='kwabeohemeng@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email,[Password], UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('Kwabena', 'Ohemeng', 'kwabeohemeng@gmail.com', '$2a$10$90t/k6JnH1/VDpPiHJ3J.OS/whzww2cLRhdUKsUPUn4Rb9fVHyZ0y', @UserTypeId, @CompanyId, GETDATE(), 1)

SET @UserTypeId = (SELECT ID FROM UserType WHERE Name='Employee')

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='jj@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'Jones', 'jj@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='jj1@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'Jones', 'jj1@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='j2@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'Stones', 'j2@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='j3@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'Bones', 'j3@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='j4@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'drone', 'j4@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='j5@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'throne', 'j5@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='j6@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'vom', 'j6@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

IF NOT EXISTS(SELECT TOP 1 1 FROM [User] where Email='j7@gmail.com')
INSERT INTO [User] (FirstName,LastName,Email, UserTypeId, CompanyId, CreatedAt, IsActive) VALUES ('John', 'saucer', 'j7@gmail.com', @UserTypeId, @CompanyId, GETDATE(), 1)

GO

IF NOT EXISTS(SELECT TOP 1 1 FROM Relationship where Name='Spouse')
INSERT INTO Relationship (Name, DisplayName,CreatedAt, IsActive) VALUES ('Spouse', 'Spouse', GETDATE(), 1)
IF NOT EXISTS(SELECT TOP 1 1 FROM Relationship where Name='Son')
INSERT INTO Relationship (Name, DisplayName,CreatedAt, IsActive) VALUES ('Son', 'Son', GETDATE(), 1)
IF NOT EXISTS(SELECT TOP 1 1 FROM Relationship where Name='Daughter')
INSERT INTO Relationship (Name, DisplayName,CreatedAt, IsActive) VALUES ('Daughter', 'Daughter', GETDATE(), 1)

GO

DECLARE @EmployeeId INT = (SELECT ID FROM [User] WHERE Email='jj@gmail.com')
DECLARE @RelationshipId INT = (SELECT ID FROM [Relationship] WHERE Name='Spouse')

IF NOT EXISTS(SELECT TOP 1 1 FROM EmployeeDependent where EmployeeId=@EmployeeId AND RelationshipId=@RelationshipId AND IsActive=1)
INSERT INTO EmployeeDependent (EmployeeId, FirstName,LastName, RelationshipId, CreatedAt, IsActive) VALUES (@EmployeeId, 'Rachel', 'Jones', @RelationshipId, GETDATE(), 1)

