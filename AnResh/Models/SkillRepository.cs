using AnResh.ViewModels;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Models
{
    public class SkillRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Skill> GetSkills()
        {
            var skills = new List<Skill>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                skills = db.Query<Skill>("SELECT * FROM Skills").ToList();
            }

            return skills;
        }

        public void Create(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Skills(SkillName) VALUES(@SkillName);";
                db.Execute(sqlQuery, skill);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "";

                sqlQuery = "DELETE FROM LearnedSkills WHERE SkillId = @id;";
                db.Execute(sqlQuery, new { id });

                sqlQuery = "DELETE FROM Skills WHERE SkillId = @id;";
                db.Execute(sqlQuery, new { id });
            }
        }

        public List<SkillViewModel> GetSkillsWithFlag(int id, out List<string> learnedSkillNames)
        {
            var skills = new List<Skill>();
            var learnedSkills = new List<LearnedSkill>();
            var skillsWithViewModel = new List<SkillViewModel>();
            string sqlQuery;

            learnedSkillNames = new List<string>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                skills = GetSkills();
                sqlQuery = "SELECT * FROM LearnedSkills WHERE EmployeeId = @id;";
                learnedSkills = db.Query<LearnedSkill>(sqlQuery, new { id }).ToList();

                for (var i = 0; i < skills.Count; i++)
                {
                    var skill = new SkillViewModel() { SkillId = skills[i].SkillId, SkillName = skills[i].SkillName };

                    for (var j = 0; j < learnedSkills.Count; j++)
                    {
                        if (skills[i].SkillId == learnedSkills[j].SkillId)
                        {
                            skill.IsLearned = true;
                            learnedSkillNames.Add(skills[i].SkillName);
                            break;
                        }

                        skill.IsLearned = false;
                    }

                    skillsWithViewModel.Add(skill);
                }
            }

            return skillsWithViewModel;
        }
    }
}