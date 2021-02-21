﻿CREATE TABLE Relationship (
	ID INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(500) NOT NULL,
	[DisplayName] VARCHAR(500) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT [PK_Relationship] PRIMARY KEY (ID)
);