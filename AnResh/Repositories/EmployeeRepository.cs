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

        private List<EmployeeViewModel> GetEmployees(PageViewModel page, IDbConnection db, string sqlQuery, string totalRowsQuery, string averageSalaryQuery, out TotalViewModel total)
        {
            var searched = page.SearchedSkills;
            var lsr = new LearnedSkillRepository();
            total = new TotalViewModel();
            var offset = page.Limit * (page.PageNumber - 1);
            int totalRows;
            int totalPages;
            int? averageSalary;
            
            var employees = db.Query<EmployeeViewModel>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();

            if (searched == null || searched.Length == 0)
            {
                foreach (var employee in employees)
                    employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

                totalRows = db.QuerySingle<int>(totalRowsQuery);
                totalPages = Math.Ceil(totalRows, page.Limit);
                averageSalary = db.QuerySingle<int?>(averageSalaryQuery);
            }
            else
            {
                var emp = new List<EmployeeViewModel>();

                foreach (var employee in employees)
                {
                    employee.Skills = lsr.GetAllByEmployeeId(employee.Id);

                    var skillIds = employee.Skills.Select(skill => skill.SkillId);
                    var check = skillIds.ToArray().Intersect(searched).Count() == searched.Length;

                    if (check)
                        emp.Add(employee);
                }

                employees = emp;
                totalRows = employees.Count;
                totalPages = Math.Ceil(totalRows, page.Limit); ;
                averageSalary = 0;
            }

            if (averageSalary == null)
                averageSalary = 0;

            total.Records = totalRows;
            total.Pages = totalPages;
            total.AverageSalary = (int)averageSalary;

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
                var averageSalaryQuery = "";
                var selectQuery = @"SELECT Employees.Id, Employees.Name, Employees.DepartmentId, Employees.Salary, Departments.Name AS DepartmentName 
                                 FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId";
                var filterByNameQuery = $"Employees.Name LIKE '{page.SearchNameQuery}%'";
                var filterByDepartmentQuery = $"Departments.Name LIKE '{page.SearchDepartmentQuery}%'";
                var orderByQuery = "";

                switch (page.SelectedSort)
                {
                    case SortingOption.ByName:
                        orderByQuery = "Employees.Name";
                        break;
                    case SortingOption.ByDepartment:
                        orderByQuery = "Departments.Name";
                        break;
                    default:
                        orderByQuery = "Employees.Name";
                        break;
                }

                sqlQuery = $"{selectQuery} WHERE {filterByNameQuery} AND {filterByDepartmentQuery} ORDER BY {orderByQuery} OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                totalRowsQuery = $"SELECT COUNT(1) FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId WHERE {filterByNameQuery} AND {filterByDepartmentQuery}";
                averageSalaryQuery = $"SELECT AVG(Salary) FROM Employees JOIN Departments ON Departments.Id = Employees.DepartmentId WHERE {filterByNameQuery} AND {filterByDepartmentQuery}";

                employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, averageSalaryQuery, out total);
            }

            return employees;
        }

        //public List<EmployeeViewModel> GetAllByDepartmentId(int id, PageViewModel page, out TotalViewModel total)
        //{
        //    var employees = new List<EmployeeViewModel>();
        //    total = new TotalViewModel();

        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        var sqlQuery = "";
        //        var totalRowsQuery = "";
        //        var averageSalaryQuery = "";
        //        var selectQuery = $"SELECT * FROM Employees WHERE DepartmentId = {id}";

        //        switch (page.SelectedSort)
        //        {
        //            case SortingOption.ByName:
        //                sqlQuery = $"{selectQuery} AND Name LIKE '%{page.SearchQuery}%' ORDER BY Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
        //                totalRowsQuery = $"SELECT COUNT(Name) FROM Employees WHERE DepartmentId = {id} AND Name LIKE '%{page.SearchQuery}%'";
        //                averageSalaryQuery = $"SELECT AVG(Salary) FROM Employees WHERE DepartmentId = {id} AND Name LIKE '%{page.SearchQuery}%'";

        //                employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, averageSalaryQuery, out total);
        //                break;
        //            case SortingOption.BySkills:
        //                sqlQuery = selectQuery;

        //                employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, averageSalaryQuery, out total);
        //                break;
        //            default:
        //                sqlQuery = $"{selectQuery} ORDER BY Id OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
        //                totalRowsQuery = $"SELECT COUNT(Id) FROM Employees WHERE DepartmentId = {id}";
        //                averageSalaryQuery = $"SELECT AVG(Salary) FROM Employees WHERE DepartmentId = {id}";

        //                employees = GetEmployees(page, db, sqlQuery, totalRowsQuery, averageSalaryQuery, out total);
        //                break;
        //        }
        //    }

        //    return employees;
        //}

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