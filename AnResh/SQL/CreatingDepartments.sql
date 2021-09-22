CREATE TABLE Departments (
DepartmentName NVARCHAR(50) NOT NULL,
DepartmentId INT IDENTITY NOT NULL PRIMARY KEY
);

INSERT INTO Departments (DepartmentName)
VALUES
('Management'),
('Financial'),
('Development');