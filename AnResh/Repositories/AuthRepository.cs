using AnResh.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AnResh.Repositories
{
    public class AuthRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void SignUp(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlQuery = "INSERT INTO Users(Name, Role, Login, Password) VALUES(@Name, @Role, @Login, @Password);";
                db.Execute(sqlQuery, user);
            }
        }
    }
}