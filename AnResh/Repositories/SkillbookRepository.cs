using Dapper;
using AnResh.Models;
using AnResh.ViewModels;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Repositories
{
    public class SkillbookRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Skill> GetAllForOne(int id)
        {
            List<Skill> skills = new List<Skill>();
            var query = @"SELECT * FROM Skills WHERE Id IN 
                         (SELECT SkillId FROM Skillbook WHERE EmployeeId = @id)";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                skills = db.Query<Skill>(query, new { id }).ToList();
            }

            return skills;
        }

        public List<SkillViewModel> GetAllForMany(int[] ids)
        {
            List<SkillViewModel> skills = new List<SkillViewModel>();
            var query = @"SELECT Skills.Id, Skills.Name, Skillbook.EmployeeId
                          FROM Skillbook 
                          JOIN Skills ON Skillbook.SkillId = Skills.Id 
                          WHERE Skillbook.EmployeeId IN @ids";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                skills = db.Query<SkillViewModel>(query, new { ids }).ToList();
            }

            return skills;
        }

        public void Update(int employeeId, int[] skillIds)
        {
            int[] skills;
            var queryGet = "SELECT SkillId FROM Skillbook WHERE EmployeeId = @employeeId";
            var queryUpdate = @"INSERT INTO Skillbook SELECT Employees.Id AS EmployeeId, Skills.Id AS SkillId
                                FROM Employees, Skills WHERE Employees.Id = @employeeId AND Skills.Id IN @toInsert
                                DELETE FROM Skillbook WHERE EmployeeId = @employeeId AND SkillId IN @toDelete";

            if (skillIds == null)
                skillIds = new int[0];

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                skills = db.Query<int>(queryGet, new { employeeId }).ToArray();

                var toInsert = skillIds.Except(skills);
                var toDelete = skills.Except(skillIds);
                var multi = db.QueryMultiple(queryUpdate, new { employeeId, toInsert, toDelete });
            }
        }
    }
}