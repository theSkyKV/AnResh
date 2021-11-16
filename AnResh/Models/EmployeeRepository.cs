using AnResh.ViewModels;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Models
{
    public class EmployeeRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<EmployeeViewModel> GetAllEmployeesWithViewModel()
        {
            var employees = new List<EmployeeViewModel>();

            List<string> learnedSkillNames;

            //это будет в более поздних этапах, но почитай про абстрактную фабрику, чтобы каждый раз не мучаться с создание коннекции
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = @"неужели
это 
позволяет
не
конкатенировать
строки
?
да нет, бред какой-то
";

                var sqlQuery = "SELECT Employees.EmployeeName, Employees.EmployeeId, Employees.Salary, Departments.DepartmentName " +
                    "FROM Employees JOIN Departments ON Departments.DepartmentId = Employees.DepartmentId;";
                employees = db.Query<EmployeeViewModel>(sqlQuery).ToList();

                var skillRepository = new SkillRepository();
                foreach (var employee in employees)
                {
                    employee.Skills = skillRepository.GetSkillsWithFlag(employee.EmployeeId, out learnedSkillNames);
                    employee.LearnedSkills = learnedSkillNames;
                }
            }

            return employees;
        }

        public void Create(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Employees(EmployeeName, DepartmentId, Salary) VALUES(@EmployeeName, @DepartmentId, @Salary);";
                db.Execute(sqlQuery, employee);
            }
        }

        public void Edit(int id, string name, int departmentId, int salary, int[] skills)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                //тоже самое
                var sqlQuery = "UPDATE Employees " +
                    "SET EmployeeName = @EmployeeName, DepartmentId = @DepartmentId, Salary = @Salary WHERE EmployeeId = @EmployeeId;";
                db.Execute(sqlQuery, new { EmployeeId = id, EmployeeName = name, DepartmentId = departmentId, Salary = salary });

                //а если переделать на обновление? добавить того, чего не было, удалить то, что стало ненужным
                //скрипт будет сложнее, но так меньше операций удаления/создания
                sqlQuery = "DELETE FROM LearnedSkills WHERE EmployeeId = @id;";
                db.Execute(sqlQuery, new { id });

                foreach (var skill in skills)
                {
                    sqlQuery = "INSERT INTO LearnedSkills(EmployeeId, SkillId) VALUES(@EmployeeId, @SkillId);";
                    db.Execute(sqlQuery, new { EmployeeId = id, SkillId = skill });
                }
            }
        }

        public void Delete(int id)
        {
            string sqlQuery;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                //это можно попробовать объединить
                sqlQuery = "DELETE FROM LearnedSkills WHERE EmployeeId = @id;";
                db.Execute(sqlQuery, new { id });

                sqlQuery = "DELETE FROM Employees WHERE EmployeeId = @id;";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}