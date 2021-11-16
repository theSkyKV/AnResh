using AnResh.ViewModels;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Models
{
    //отдели мух от котлет: доменные модели отдельно, репозитарии отдельно
    public class DepartmentRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Department> GetDepartments()
        {
            var departments = new List<Department>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                departments = db.Query<Department>("SELECT * FROM Departments").ToList();
            }

            //это нужно посчитать в БД, будет на порядок быстрее
            foreach (var department in departments)
                department.SetAverageSalary();

            return departments;
        }

        public DepartmentViewModel GetDepartmentWithViewModel(int id)
        {
            DepartmentViewModel department;
            List<string> learnedSkillNames;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT Employees.EmployeeName, Employees.EmployeeId, Employees.DepartmentId, Employees.Salary, Departments.DepartmentName " +
                    "FROM Employees JOIN Departments ON Departments.DepartmentId = Employees.DepartmentId WHERE Employees.DepartmentId = @id;";
                var employees = db.Query<EmployeeViewModel>(sqlQuery, new { id }).ToList();

                var skillRepository = new SkillRepository();
                foreach (var employee in employees)
                {
                    employee.Skills = skillRepository.GetSkillsWithFlag(employee.EmployeeId, out learnedSkillNames);
                    employee.LearnedSkills = learnedSkillNames;
                }

                var name = db.QuerySingle<string>("SELECT DepartmentName FROM Departments WHERE DepartmentId = @id;", new { id });
                department = new DepartmentViewModel() { DepartmentId = id, DepartmentName = name, Employees = employees };
            }

            return department;
        }

        public void Create(Department department)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Departments(DepartmentName, AverageSalary) " +
                    "VALUES(@DepartmentName, 0);";
                db.Execute(sqlQuery, department);
            }
        }

        public void Edit(Department department)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Departments SET DepartmentName = @DepartmentName WHERE DepartmentId = @DepartmentId;";
                db.Execute(sqlQuery, department);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Departments WHERE DepartmentId = @id;";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}