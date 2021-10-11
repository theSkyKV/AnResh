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

        public List<Employee> GetEmployees()
        {
            var employees = new List<Employee>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                employees = db.Query<Employee>("SELECT * FROM Employees").ToList();
            }

            return employees;
        }

        public List<IndexViewModel> GetAllEmployees()
        {
            var employees = new List<IndexViewModel>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                employees = db.Query<IndexViewModel>("SELECT Employees.EmployeeId, Employees.EmployeeName, " +
                    "Employees.DepartmentId, Employees.Salary, Departments.DepartmentName " +
                    "FROM Employees JOIN Departments ON Departments.DepartmentId=Employees.DepartmentId;").ToList();
            }

            return employees;
        }

        public List<Employee> GetEmployeesByDepartmentId(int id)
        {
            var employees = new List<Employee>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Employees WHERE DepartmentId = @id";
                employees = db.Query<Employee>(sqlQuery, new { id }).ToList();
            }
            return employees;
        }

        public List<IndexViewModel> GetAllEmployeesByDepartmentId(int id)
        {
            var employees = new List<IndexViewModel>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT Employees.EmployeeId, Employees.EmployeeName, " +
                    "Employees.DepartmentId, Employees.Salary, Departments.DepartmentName " +
                    "FROM Employees JOIN Departments ON Departments.DepartmentId=Employees.DepartmentId WHERE Employees.DepartmentId = @id;";
                employees = db.Query<IndexViewModel>(sqlQuery, new { id }).ToList();
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

        public void Edit(Employee employee)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Employees SET EmployeeName = @EmployeeName, " +
                    "DepartmentId = @DepartmentId, Salary = @Salary " +
                    "WHERE EmployeeId = @EmployeeId;";
                db.Execute(sqlQuery, employee);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Employees WHERE EmployeeId = @id;";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}