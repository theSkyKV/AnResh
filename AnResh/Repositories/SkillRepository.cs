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

        public List<Skill> GetAll()
        {
            var skills = new List<Skill>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                skills = db.Query<Skill>("SELECT * FROM Skills").ToList();
            }

            return skills;
        }

        public Skill GetById(int id)
        {
            Skill skill;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "SELECT * FROM Skills WHERE Id = @id";
                skill = db.QuerySingle<Skill>(sqlQuery, new { id });
            }

            return skill;
        }

        public void Create(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Skills(Name) VALUES(@Name);";
                db.Execute(sqlQuery, skill);
            }
        }

        public void Edit(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "UPDATE Skills SET Name = @Name WHERE Id = @Id;";
                db.Execute(sqlQuery, skill);
            }
        }

        public void Delete(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "DELETE FROM Skills WHERE Id = @Id;";
                db.Execute(sqlQuery, skill);
            }
        }
    }
}