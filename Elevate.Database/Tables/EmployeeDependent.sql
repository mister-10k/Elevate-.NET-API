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