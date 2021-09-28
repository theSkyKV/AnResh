CREATE TABLE Departments (
DepartmentName NVARCHAR(50) NOT NULL,
AverageSalary REAL NOT NULL,
DepartmentId INT IDENTITY NOT NULL PRIMARY KEY
);

INSERT INTO Departments (DepartmentName, AverageSalary)
VALUES
('Management', 0),
('Financial', 0),
('Development', 0);