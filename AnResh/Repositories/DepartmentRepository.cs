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
    public class DepartmentRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<DepartmentViewModel> GetAll()
        {
            var departments = new List<DepartmentViewModel>();
            int? averageSalary;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Departments";
                departments = db.Query<DepartmentViewModel>(sqlQuery).ToList();

                foreach (var department in departments)
                {
                    sqlQuery = "SELECT AVG(Salary) FROM Employees WHERE Employees.DepartmentId = @id";
                    averageSalary = db.QuerySingle<int?>(sqlQuery, new { id = department.Id });

                    if (averageSalary == null)
                    {
                        department.AverageSalary = 0;
                        continue;
                    }

                    department.AverageSalary = (int)averageSalary;
                }
            }

            return departments;
        }

        public Department GetById(int id)
        {
            Department department;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Departments WHERE Id = @id";
                department = db.QuerySingle<Department>(sqlQuery, new { id });
            }

            return department;
        }

        public void Create(Department department)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Departments(Name) VALUES(@Name);";
                db.Execute(sqlQuery, department);
            }
        }

        public void Edit(Department department)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Departments SET Name = @Name WHERE Id = @Id;";
                db.Execute(sqlQuery, department);
            }
        }

        public void Delete(Department department)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Departments WHERE Id = @Id;";
                db.Execute(sqlQuery, department);
            }
        }
    }
}