using AnResh.HelperFunctions;
using AnResh.Models;
using AnResh.ViewModels;
using Dapper;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Authentication;

namespace AnResh.AuthenticationServer
{
    public class AuthServer
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private const string SECRET_KEY = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public string SecretKey => SECRET_KEY;

        public string SignIn(SignInViewModel auth)
        {
            var user = GetUser(auth);

            if (user == null)
                throw new AuthenticationException("Check name or password");

            var claims = GetClaims(user);
            var token = GetToken(claims);
            return token;
        }

        public void SignUp(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                user.Password = Hash.StringToHash(user.Password);
                var sqlQuery = $"INSERT INTO Users(Name, Role, Login, Password) VALUES(@Name, @Role, @Login, @Password);";
                db.Execute(sqlQuery, user);
            }
        }

        private User GetUser(SignInViewModel auth)
        {
            User user;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                auth.Password = Hash.StringToHash(auth.Password);
                var sqlQuery = $"SELECT * FROM Users WHERE Login = @Login AND Password = @Password";
                user = db.QueryFirstOrDefault<User>(sqlQuery, auth);
            }

            return user;
        }

        private List<KeyValuePair<string, object>> GetClaims(User user)
        {
            var claims = new List<KeyValuePair<string, object>>();

            claims.Add(new KeyValuePair<string, object>("UserId", user.Id));
            claims.Add(new KeyValuePair<string, object>("Name", user.Name));
            claims.Add(new KeyValuePair<string, object>("Role", user.Role));
            claims.Add(new KeyValuePair<string, object>("Login", user.Login));

            return claims;
        }

        private string GetToken(List<KeyValuePair<string, object>> claims)
        {
            var token = JwtBuilder
                                .Create()
                                .WithAlgorithm(new HMACSHA256Algorithm())
                                .WithSecret(SECRET_KEY)
                                .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                                .AddClaims(claims)
                                .Encode();
            return token;
        }
    }
}