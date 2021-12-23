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
    public class EmployeeRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //public List<EmployeeViewModel> GetAll()
        //{
        //    var lsr = new LearnedSkillRepository();
        //    var employees = new List<EmployeeViewModel>();

        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        var sqlQuery = @"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Salary, Departments.Name AS DepartmentName 
        //                         FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId";
        //        employees = db.Query<EmployeeViewModel>(sqlQuery).ToList();
        //    }

        //    foreach (var employee in employees)
        //        employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

        //    return employees;
        //}

        private void Meth(PageViewModel page, IDbConnection db, string sqlQuery, string totalRowsQuery)
        {
            var lsr = new LearnedSkillRepository();
            var employees = new List<EmployeeViewModel>();
            var offset = page.Limit * (page.PageNumber - 1);

            employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

            foreach (var employee in employees)
                employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

            var totalRows = db.QuerySingle<int>(totalRowsQuery);
            var totalPages = Math.Ceil(totalRows, page.Limit);
        }

        public List<EmployeeViewModel> GetAll(PageViewModel page, out int totalPages)
        {
            var lsr = new LearnedSkillRepository();
            var employees = new List<EmployeeViewModel>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var totalRows = 0;
                var sqlQuery = "";
                var selectQuery = @"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Salary, Departments.Name AS DepartmentName 
                                 FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId";
                var totalRowsQuery = "";
                var offset = page.Limit * (page.PageNumber - 1);

                switch (page.SelectedSort)
                {
                    case SortingOption.ByName:
                        sqlQuery = $"{selectQuery} WHERE Employees.Name LIKE '%{page.SearchQuery}%' ORDER BY Employees.Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Name) FROM Employees WHERE Name LIKE '%{page.SearchQuery}%'";

                        employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

                        foreach (var employee in employees)
                            employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

                        totalRows = db.QuerySingle<int>(totalRowsQuery);
                        break;
                    case SortingOption.ByDepartment:
                        sqlQuery = $"{selectQuery} WHERE Departments.Name LIKE '%{page.SearchQuery}%' ORDER BY Departments.Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Employees.Id) FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId WHERE Departments.Name LIKE '%{page.SearchQuery}%'";

                        employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

                        foreach (var employee in employees)
                            employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

                        totalRows = db.QuerySingle<int>(totalRowsQuery);
                        break;
                    case SortingOption.BySkills:
                        sqlQuery = selectQuery;
                        employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();
                        var empls = new List<EmployeeViewModel>();

                        if (page.SearchQuery == null)
                            page.SearchQuery = "";

                        foreach (var employee in employees)
                        {
                            employee.Skills = lsr.GetAllByEmployeeId(employee.Id);
                            var skills = employee.Skills.Where(sk => sk.SkillName.ToLower().Contains(page.SearchQuery.ToLower())).ToList();

                            if (skills.Count > 0)
                                empls.Add(employee);
                        }

                        totalRows = empls.ToList().Count;
                        employees = empls.Skip(offset).Take(page.Limit).ToList();
                        break;
                    default:
                        sqlQuery = $"{selectQuery} ORDER BY Id OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = "SELECT COUNT(Id) FROM Employees";

                        employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

                        foreach (var employee in employees)
                            employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

                        totalRows = db.QuerySingle<int>(totalRowsQuery);
                        break;
                }

                totalPages = Math.Ceil(totalRows, page.Limit);
            }

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
                var sqlQuery = "INSERT INTO Employees(Name, DepartmentId, Salary) VALUES(@Name, @DepartmentId, @Salary);";
                db.Execute(sqlQuery, employee);
            }
        }

        public void Edit(EmployeeEditViewModel employee)
        {
            var lsr = new LearnedSkillRepository();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Employees SET Name = @Name, DepartmentId = @DepartmentId, Salary = @Salary WHERE Id = @Id;";
                db.Execute(sqlQuery, employee);
            }

            lsr.Update(employee.Id, employee.Skills);
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