using AnResh.HelperFunctions;
using AnResh.Models;
using AnResh.ViewModels;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Repositories
{
    public class EmployeeRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<EmployeeViewModel> GetAll(PageViewModel page, out int totalPages)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            var totalRows = 0;
            var offset = page.Limit * (page.PageNumber - 1);

            var query = $@"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Salary, Departments.Name AS DepartmentName
                               FROM Employees 
                               JOIN Departments ON Departments.Id = Employees.DepartmentId
                               WHERE Employees.Name LIKE '{page.SearchName}%' AND Departments.Name LIKE '{page.SearchDepartment}%' {GetSearchSkillsQuery(page.SearchSkills)}
                               ORDER BY {page.OrderBy} OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY
                               SELECT COUNT(1) FROM Employees
                               JOIN Departments ON Departments.Id = Employees.DepartmentId
                               WHERE Employees.Name LIKE '{page.SearchName}%' AND Departments.Name LIKE '{page.SearchDepartment}%' {GetSearchSkillsQuery(page.SearchSkills)}";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var multi = db.QueryMultiple(query, new { offset = offset, limit = page.Limit, skills = page.SearchSkills });
                employees = multi.Read<EmployeeViewModel>().ToList();
                totalRows = multi.Read<int>().Single();
            }

            totalPages = Math.Ceil(totalRows, page.Limit);

            return employees;
        }

        public List<EmployeeViewModel> GetAllByDepartmentId(int id, PageViewModel page, out int totalPages)
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

            var totalRows = 0;
            var offset = page.Limit * (page.PageNumber - 1);

            var query = $@"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Salary, Departments.Name AS DepartmentName
                           FROM Employees 
                           JOIN Departments ON Departments.Id = Employees.DepartmentId
                           WHERE DepartmentId = @id AND Employees.Name LIKE '{page.SearchName}%' {GetSearchSkillsQuery(page.SearchSkills)}
                           ORDER BY {page.OrderBy} OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY
                           SELECT COUNT(1) FROM Employees 
                           WHERE DepartmentId = @id AND Employees.Name LIKE '{page.SearchName}%' {GetSearchSkillsQuery(page.SearchSkills)}";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var multi = db.QueryMultiple(query, new { id = id, offset = offset, limit = page.Limit, skills = page.SearchSkills });
                employees = multi.Read<EmployeeViewModel>().ToList();
                totalRows = multi.Read<int>().Single();
            }

            totalPages = Math.Ceil(totalRows, page.Limit);

            return employees;
        }

        public Employee GetById(int id)
        {
            Employee employee;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Employees WHERE Id = @id";
                employee = db.QuerySingleOrDefault<Employee>(query, new { id });
            }

            return employee;
        }

        public void Create(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Employees(Name, DepartmentId, Salary) VALUES(@Name, @DepartmentId, @Salary);";
                db.Execute(query, employee);
            }
        }

        public void Edit(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Employees SET Name = @Name, DepartmentId = @DepartmentId, Salary = @Salary WHERE Id = @Id;";
                db.Execute(query, employee);
            }
        }

        public void Delete(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Employees WHERE Id = @Id;";
                db.Execute(query, employee);
            }
        }

        private string GetSearchSkillsQuery(int[] skills)
        {
            if (skills == null || skills.Length == 0)
                return "";

            return $@"AND (SELECT COUNT(1) FROM (SELECT Skillbook.SkillId AS SkillId FROM Skillbook
                       WHERE Skillbook.EmployeeId = Employees.Id
                       INTERSECT
                       SELECT Skills.Id AS SkillId FROM Skills
                       WHERE Skills.Id IN @skills)SkillId) 
                       = 
                      (SELECT COUNT(1) FROM Skills WHERE Skills.Id IN @skills)";
        }
    }
}