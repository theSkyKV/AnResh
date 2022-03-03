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
    public class DepartmentRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Department> GetAll(PageViewModel page, out int totalPages)
        {
            List<Department> departments = new List<Department>();

            var totalRows = 0;
            var offset = page.Limit * (page.PageNumber - 1);

            var query = $@"SELECT * FROM Departments 
                           WHERE Departments.Name LIKE '{page.SearchName}%'
                           ORDER BY {page.OrderBy} OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY
                           SELECT COUNT(1) FROM Departments
                           WHERE Departments.Name LIKE '{page.SearchName}%'";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var multi = db.QueryMultiple(query, new { offset = offset, limit = page.Limit });
                departments = multi.Read<Department>().ToList();
                totalRows = multi.Read<int>().Single();
            }

            totalPages = Math.Ceil(totalRows, page.Limit);

            return departments;
        }

        public Department GetById(int id)
        {
            Department department;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Departments WHERE Id = @id";
                department = db.QuerySingleOrDefault<Department>(sqlQuery, new { id });
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