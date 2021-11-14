CREATE TABLE LearnedSkills (
Id INT IDENTITY NOT NULL PRIMARY KEY,
EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(EmployeeId),
SkillId INT NOT NULL FOREIGN KEY REFERENCES Skills(SkillId)
);

INSERT INTO LearnedSkills (EmployeeId, SkillId)
VALUES
(1, 1),
(1, 2),
(2, 3),
(3, 2),
(4, 4),
(5, 2),
(6, 5),
(7, 2),
(8, 2),
(9, 1),
(9, 3),
(10, 3);