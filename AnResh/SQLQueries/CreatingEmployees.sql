CREATE TABLE Employees (
EmployeeId INT IDENTITY NOT NULL PRIMARY KEY,
EmployeeName NVARCHAR(50) NOT NULL,
DepartmentId INT NOT NULL,
Salary INT NOT NULL
);

INSERT INTO Employees (EmployeeName, DepartmentId, Salary)
VALUES
('John Jones', 2, 1000),
('Daniel Cormier', 1, 1500),
('Habib Nurmagomedov', 2, 1100),
('Amanda Nunes', 3, 1200),
('Valentina Shevchenko', 3, 1250),
('Stipe Miocic', 1, 1450),
('Anthony Johnson', 3, 1250),
('Ronda Rousey', 2, 1150),
('Cain Velasquez', 2, 1200),
('Jose Aldo', 3, 1200);