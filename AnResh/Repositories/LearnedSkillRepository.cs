using Dapper;
using AnResh.Models;
using AnResh.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AnResh.Repositories
{
    public class LearnedSkillRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<LearnedSkillViewModel> GetAllByEmployeeId(int id)
        {
            var skills = new List<LearnedSkillViewModel>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = @"SELECT LearnedSkills.Id, LearnedSkills.EmployeeId, LearnedSkills.SkillId, Skills.Name AS SkillName 
                                 FROM LearnedSkills JOIN Skills ON Skills.Id = LearnedSkills.SkillId WHERE LearnedSkills.EmployeeId = @id";
                skills = db.Query<LearnedSkillViewModel>(sqlQuery, new { id }).ToList();
            }

            return skills;
        }

        public void Update(int employeeId, int[] learnedSkills)
        {
            if (learnedSkills == null)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    db.Execute("DELETE FROM LearnedSkills WHERE EmployeeId = @employeeId", new { employeeId = employeeId });
                }

                return;
            }

            var learnedSkillsString = string.Join(",", learnedSkills);

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQueryAlreadyLearned = "SELECT SkillId AS Id FROM LearnedSkills WHERE EmployeeId = @employeeId";
                var sqlQueryNewSkills = "SELECT Id FROM Skills WHERE Id IN (SELECT CAST(Item AS INTEGER) FROM dbo.SplitString(@skillsId, ','))";
                var sqlQuerySkillsToInsert = $"{sqlQueryNewSkills} EXCEPT {sqlQueryAlreadyLearned}";
                var sqlQuerySkillsToDelete = $"{sqlQueryAlreadyLearned} EXCEPT {sqlQueryNewSkills}";
                var sqlQueryInsert = "INSERT INTO LearnedSkills(EmployeeId, SkillId) VALUES(@employeeId, @skillId)";
                var sqlQueryDelete = $"DELETE FROM LearnedSkills WHERE EmployeeId = @employeeId AND SkillId IN ({sqlQuerySkillsToDelete})";

                var skillsToInsert = db.Query<int>(sqlQuerySkillsToInsert, new { employeeId = employeeId, skillsId = learnedSkillsString }).ToList();
                db.Execute(sqlQueryDelete, new { employeeId = employeeId, skillsId = learnedSkillsString });

                foreach (var skill in skillsToInsert)
                    db.Execute(sqlQueryInsert, new { employeeId = employeeId, skillId = skill });
            }
        }
    }
}