﻿using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Models
{
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

            foreach (var department in departments)
                department.SetAverageSalary();

            return departments;
        }

        public void Create(Department department)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Departments(DepartmentName, AverageSalary) VALUES(@DepartmentName, 0);";
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