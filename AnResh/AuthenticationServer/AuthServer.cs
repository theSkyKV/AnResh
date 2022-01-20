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
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AnResh.AuthenticationServer
{
    public class AuthServer
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private string _secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public string SecretKey => _secretKey;

        public string Auth(SignInViewModel auth, out Dictionary<string, object> payload)
        {
            var user = GetUser(auth);

            if (user == null)
            {
                payload = null;
                return null;
            }

            var claims = GetClaims(user);
            var token = GetToken(claims);
            payload = claims.ToDictionary(x => x.Key, x => x.Value);

            return token;
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

            claims.Add(new KeyValuePair<string,object>("Name", user.Name));
            claims.Add(new KeyValuePair<string, object>("Role", user.Role));

            return claims;
        }

        private string GetToken(List<KeyValuePair<string, object>> claims)
        {
            var token = JwtBuilder
                                .Create()
                                .WithAlgorithm(new HMACSHA256Algorithm())
                                .WithSecret(_secretKey)
                                .AddClaims(claims)
                                .Encode();
            return token;
        }
    }
}