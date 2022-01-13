using AnResh.Models;
using AnResh.ViewModels;
using Dapper;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
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

        //public string Auth(SignInViewModel auth)
        //{
        //    var user = GetUser(auth);

        //    if (user == null)
        //        return null;

        //    var claims = GetClaims(user);
        //    var token = GetToken(claims);

        //    return token;
        //}

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
                var sqlQuery = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";
                user = db.QueryFirstOrDefault<User>(sqlQuery, auth);
            }

            return user;
        }

        //private List<Claim> GetClaims(User user)
        //{
        //    var claims = new List<Claim>();

        //    claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name));
        //    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role));

        //    return claims;
        //}

        private List<KeyValuePair<string, object>> GetClaims(User user)
        {
            var claims = new List<KeyValuePair<string, object>>();

            claims.Add(new KeyValuePair<string,object>("Name", user.Name));
            claims.Add(new KeyValuePair<string, object>("Role", user.Role));

            return claims;
        }

        //private string GetToken(List<Claim> claims)
        //{
        //    var payload = claims;

        //    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        //    IJsonSerializer serializer = new JsonNetSerializer();
        //    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        //    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        //    var token = encoder.Encode(payload, _secretKey);
        //    return token;
        //}

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

        //public bool CheckToken(HttpContextBase httpContext, out Dictionary<string, object> payload)
        //{
        //    try
        //    {
        //        var token = httpContext.Request.Headers["Authorization"].ToString();

        //        IJsonSerializer serializer = new JsonNetSerializer();
        //        IDateTimeProvider provider = new UtcDateTimeProvider();
        //        IJwtValidator validator = new JwtValidator(serializer, provider);
        //        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        //        IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        //        IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

        //        //decoder.Decode(token, _secretKey, verify: true);
        //        payload = decoder.DecodeToObject<Dictionary<string, object>>(token, _secretKey, true);
        //        return true;
        //    }
        //    catch
        //    {
        //        payload = null;
        //        return false;
        //    }
        //}

        //public string GetToken(SignInViewModel auth)
        //{
        //    var identity = GetIdentity(auth);
        //    if (identity == null)
        //    {
        //        return null;
        //    }

        //    var payload = new Dictionary<string, object>();

        //    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        //    IJsonSerializer serializer = new JsonNetSerializer();
        //    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        //    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        //    var token = encoder.Encode(payload, _secretKey);
        //    return token;
        //}

        //private ClaimsIdentity GetIdentity(SignInViewModel auth)
        //{
        //    User user;

        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        var sqlQuery = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";
        //        user = db.QuerySingle<User>(sqlQuery, auth);
        //    }

        //    if (user != null)
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
        //            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
        //        };

        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "GetToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //        return claimsIdentity;
        //    }

        //    return null;
        //}
    }
}