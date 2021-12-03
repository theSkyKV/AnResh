using AnResh.Repositories;
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

        public List<EmployeeViewModel> GetAll()
        {
            var lsr = new LearnedSkillRepository();
            var employees = new List<EmployeeViewModel>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = @"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Power, Departments.Name AS DepartmentName 
                                 FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId";
                employees = db.Query<EmployeeViewModel>(sqlQuery).ToList();
            }

            foreach (var employee in employees)
                employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

            return employees;
        }

        public List<EmployeeViewModel> GetAllByDepartmentId(int id)
        {
            var lsr = new LearnedSkillRepository();
            var employees = new List<EmployeeViewModel>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Employees WHERE DepartmentId = @id";
                employees = db.Query<EmployeeViewModel>(sqlQuery, new { id }).ToList();
            }

            foreach (var employee in employees)
                employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

            return employees;
        }

        public EmployeeViewModel GetById(int id)
        {
            var lsr = new LearnedSkillRepository();
            EmployeeViewModel employee;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Employees WHERE Id = @id";
                employee = db.QuerySingle<EmployeeViewModel>(sqlQuery, new { id });
            }

            employee.Skills = lsr.GetAllByEmployeeId(id);

            return employee;
        }

        public void Create(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Employees(Name, DepartmentId, Power) VALUES(@Name, @DepartmentId, @Power);";
                db.Execute(sqlQuery, employee);
            }
        }

        public void Edit(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Employees SET Name = @Name, DepartmentId = @DepartmentId, Power = @Power WHERE Id = @Id;";
                db.Execute(sqlQuery, employee);
            }
        }

        public void Delete(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Employees WHERE Id = @Id;";
                db.Execute(sqlQuery, employee);
            }
        }
    }
}