CREATE TABLE Skills (
SkillName NVARCHAR(50) NOT NULL,
SkillId INT IDENTITY NOT NULL PRIMARY KEY
);

INSERT INTO Skills (SkillName)
VALUES
('.Net MVC'),
('VueJS'),
('C#'),
('CSS'),
('HTML');