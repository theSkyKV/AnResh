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
    }
}