using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AnResh.Models
{
    public static class DapperORM
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static List<T> GetAllItemsByType<T>()
        {
            var items = new List<T>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                items = db.Query<T>($"SELECT * FROM {typeof(T).Name}s").ToList();
            }
            return items;
        }

        public static List<T> GetAllItemsById<T>(int id)
        {
            var items = new List<T>();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                items = db.Query<T>($"SELECT * FROM {typeof(T).Name}s WHERE DepartmentId = {id}").ToList();
            }
            return items;
        }
    }
}