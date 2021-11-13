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

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT Employees.EmployeeName, Employees.EmployeeId, Employees.Salary, Departments.DepartmentName " +
                    "FROM Employees JOIN Departments ON Departments.DepartmentId = Employees.DepartmentId;";
                employees = db.Query<EmployeeViewModel>(sqlQuery).ToList();
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
                var sqlQuery = "UPDATE Employees " +
                    "SET EmployeeName = @EmployeeName, DepartmentId = @DepartmentId, Salary = @Salary WHERE EmployeeId = @EmployeeId;";
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