﻿CREATE TABLE UserType (
	ID INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(300) NOT NULL,
	[DisplayName] VARCHAR(500) NOT NULL,
	CreatedAt DATETIME NOT NULL,
	ModifiedAt DATETIME NULL,
	IsActive BIT NOT NULL,
	CONSTRAINT [PK_UserType] PRIMARY KEY (ID)
);