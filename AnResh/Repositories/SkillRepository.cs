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
    public class SkillRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Skill> GetAll(PageViewModel page, out int totalPages)
        {
            List<Skill> skills = new List<Skill>();

            var totalRows = 0;
            var offset = page.Limit * (page.PageNumber - 1);

            var query = $@"SELECT * FROM Skills 
                           WHERE Skills.Name LIKE '{page.SearchName}%'
                           ORDER BY {page.OrderBy} OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY
                           SELECT COUNT(1) FROM Skills
                           WHERE Skills.Name LIKE '{page.SearchName}%'";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var multi = db.QueryMultiple(query, new { offset = offset, limit = page.Limit });
                skills = multi.Read<Skill>().ToList();
                totalRows = multi.Read<int>().Single();
            }

            totalPages = Math.Ceil(totalRows, page.Limit);

            return skills;
        }

        public Skill GetById(int id)
        {
            Skill skill;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Skills WHERE Id = @id";
                skill = db.QuerySingleOrDefault<Skill>(query, new { id });
            }

            return skill;
        }

        public void Create(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Skills(Name) VALUES(@Name);";
                db.Execute(query, skill);
            }
        }

        public void Edit(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Skills SET Name = @Name WHERE Id = @Id;";
                db.Execute(query, skill);
            }
        }

        public void Delete(Skill skill)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Skills WHERE Id = @Id;";
                db.Execute(query, skill);
            }
        }
    }
}