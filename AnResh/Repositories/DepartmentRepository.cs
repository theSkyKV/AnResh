using AnResh.Enums;
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

        //public List<DepartmentViewModel> GetAll()
        //{
        //    var departments = new List<DepartmentViewModel>();
        //    int? averageSalary;

        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        var sqlQuery = "SELECT * FROM Departments";
        //        departments = db.Query<DepartmentViewModel>(sqlQuery).ToList();

        //        foreach (var department in departments)
        //        {
        //            sqlQuery = "SELECT AVG(Salary) FROM Employees WHERE Employees.DepartmentId = @id";
        //            averageSalary = db.QuerySingle<int?>(sqlQuery, new { id = department.Id });

        //            if (averageSalary == null)
        //            {
        //                department.AverageSalary = 0;
        //                continue;
        //            }

        //            department.AverageSalary = (int)averageSalary;
        //        }
        //    }

        //    return departments;
        //}

        public List<DepartmentViewModel> GetAll(PageViewModel page, out TotalViewModel total)
        {
            var departments = new List<DepartmentViewModel>();
            total = new TotalViewModel();
            int? averageSalary;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var totalRows = 0;
                var sqlQuery = "";
                var totalRowsQuery = "";
                var offset = page.Limit * (page.PageNumber - 1);

                switch (page.SelectedSort)
                {
                    case SortingOption.ByName:
                        sqlQuery = $"SELECT * FROM Departments WHERE Name LIKE '%{page.SearchQuery}%' ORDER BY Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Name) FROM Departments WHERE Name LIKE '%{page.SearchQuery}%'";
                        break;
                    default:
                        sqlQuery = "SELECT * FROM Departments ORDER BY Id OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = "SELECT COUNT(Id) FROM Departments";
                        break;
                }

                departments = db.Query<DepartmentViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

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

                totalRows = db.QuerySingle<int>(totalRowsQuery);
                var totalPages = Math.Ceil(totalRows, page.Limit);

                total.Records = totalRows;
                total.Pages = totalPages;
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