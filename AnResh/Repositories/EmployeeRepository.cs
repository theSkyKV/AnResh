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

        private List<EmployeeViewModel> GetEmployees(PageViewModel page, IDbConnection db, string sqlQuery, string totalRowsQuery, out TotalViewModel total)
        {
            var lsr = new LearnedSkillRepository();
            total = new TotalViewModel();
            var offset = page.Limit * (page.PageNumber - 1);
            var totalRows = 0;
            var totalPages = 0;

            var employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

            switch (page.SelectedSort)
            {
                case SortingOption.BySkills:
                    var empls = new List<EmployeeViewModel>();

                    if (page.SearchQuery == null)
                        page.SearchQuery = "";

                    foreach (var employee in employees)
                    {
                        employee.Skills = lsr.GetAllByEmployeeId(employee.Id);
                        var skills = employee.Skills.Where(skill => skill.SkillName.ToLower().Contains(page.SearchQuery.ToLower())).ToList();

                        if (skills.Count > 0)
                            empls.Add(employee);
                    }

                    totalRows = empls.ToList().Count;
                    totalPages = Math.Ceil(totalRows, page.Limit);
                    employees = empls.OrderBy(emp => emp.Skills[0].SkillName).Skip(offset).Take(page.Limit).ToList();
                    break;
                default:
                    foreach (var employee in employees)
                        employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

                    totalRows = db.QuerySingle<int>(totalRowsQuery);
                    totalPages = Math.Ceil(totalRows, page.Limit);
                    break;
            }

            total.Records = totalRows;
            total.Pages = totalPages;

            return employees;
        }

        public List<EmployeeViewModel> GetAll(PageViewModel page, out TotalViewModel total)
        {
            var employees = new List<EmployeeViewModel>();
            total = new TotalViewModel();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "";
                var totalRowsQuery = "";
                var selectQuery = @"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Salary, Departments.Name AS DepartmentName 
                                 FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId";

                switch (page.SelectedSort)
                {
                    case SortingOption.ByName:
                        sqlQuery = $"{selectQuery} WHERE Employees.Name LIKE '%{page.SearchQuery}%' ORDER BY Employees.Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Name) FROM Employees WHERE Name LIKE '%{page.SearchQuery}%'";

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                    case SortingOption.ByDepartment:
                        sqlQuery = $"{selectQuery} WHERE Departments.Name LIKE '%{page.SearchQuery}%' ORDER BY Departments.Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Employees.Id) FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId WHERE Departments.Name LIKE '%{page.SearchQuery}%'";

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                    case SortingOption.BySkills:
                        sqlQuery = selectQuery;

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                    default:
                        sqlQuery = $"{selectQuery} ORDER BY Id OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = "SELECT COUNT(Id) FROM Employees";

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                }
            }

            return employees;
        }

        public List<EmployeeViewModel> GetAllByDepartmentId(int id, PageViewModel page, out TotalViewModel total)
        {
            var employees = new List<EmployeeViewModel>();
            total = new TotalViewModel();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "";
                var totalRowsQuery = "";
                var selectQuery = $"SELECT * FROM Employees WHERE DepartmentId = {id}";

                switch (page.SelectedSort)
                {
                    case SortingOption.ByName:
                        sqlQuery = $"{selectQuery} AND Name LIKE '%{page.SearchQuery}%' ORDER BY Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Name) FROM Employees WHERE DepartmentId = {id} AND Name LIKE '%{page.SearchQuery}%'";

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                    case SortingOption.BySkills:
                        sqlQuery = selectQuery;

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                    default:
                        sqlQuery = $"{selectQuery} ORDER BY Id OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                        totalRowsQuery = $"SELECT COUNT(Id) FROM Employees WHERE DepartmentId = {id}";

                        employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, out total);
                        break;
                }
            }

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