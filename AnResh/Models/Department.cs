using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Models
{
    public class Department
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public float AverageSalary { get; private set; }

        // зачем смешивать зоны ответственности? модель не должна ходить в БД
        public void SetAverageSalary()
        {
            var employees = new List<Employee>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Employees WHERE DepartmentId = @DepartmentId";
                employees = db.Query<Employee>(sqlQuery, new { DepartmentId }).ToList();
            }

            try
            {
                AverageSalary = 0;
                foreach (var employee in employees)
                    AverageSalary += employee.Salary;

                AverageSalary /= employees.Count;
            }
            catch
            {
                AverageSalary = 0;
            }
        }
    }
}