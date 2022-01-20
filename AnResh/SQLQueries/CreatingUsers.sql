﻿IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
CREATE TABLE Users (
Id INT IDENTITY NOT NULL PRIMARY KEY,
Name NVARCHAR(50) NOT NULL,
Role NVARCHAR(50) NOT NULL,
Login NVARCHAR(50) NOT NULL UNIQUE,
Password NVARCHAR(100) NOT NULL
);