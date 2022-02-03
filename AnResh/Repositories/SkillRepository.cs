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
    public class SkillRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Skill> GetAll(PageViewModel page, out TotalViewModel total)
        {
            var skills = new List<Skill>();
            total = new TotalViewModel();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var totalRows = 0;
                var sqlQuery = "";
                var totalRowsQuery = "";
                var offset = page.Limit * (page.PageNumber - 1);

                sqlQuery = $"SELECT * FROM Skills WHERE Name LIKE '{page.SearchQuery}%' ORDER BY Name OFFSET @offset ROWS FETCH FIRST @limit ROWS ONLY";
                totalRowsQuery = $"SELECT COUNT(1) FROM Skills WHERE Name LIKE '{page.SearchQuery}%'";

                skills = db.Query<Skill>(sqlQuery, new { offset = offset, limit = page.Limit }).ToList();
                totalRows = db.QuerySingle<int>(totalRowsQuery);
                var totalPages = Math.Ceil(totalRows, page.Limit);

                total.Records = totalRows;
                total.Pages = totalPages;
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