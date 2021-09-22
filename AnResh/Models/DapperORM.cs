using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AnResh.Models
{
    public static class DapperORM
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static List<T> GetAllElementsByType<T>()
        {
            var elements = new List<T>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                elements = db.Query<T>($"SELECT * FROM {typeof(T).Name}s").ToList();
            }
            return elements;
        }

        public static List<T1> GetAllElementsFromById<T1, T2>(int id)
        {
            var elements = new List<T1>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                elements = db.Query<T1>($"SELECT * FROM {typeof(T1).Name}s WHERE {typeof(T2).Name}Id = {id}").ToList();
            }
            return elements;
        }
    }
}