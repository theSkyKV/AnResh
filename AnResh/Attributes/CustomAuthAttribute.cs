using AnResh.AuthenticationServer;
using JWT.Algorithms;
using JWT.Builder;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;

namespace AnResh.Attributes
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {
        //     Вызывается, когда процесс запрашивает авторизацию.
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        //     Предоставляет точку входа для пользовательской проверки авторизации.
        // Возврат:
        //     Значение true, если пользователь авторизован. В противном случае — значение false.
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authServer = new AuthServer();
            var secretKey = authServer.SecretKey;

            try
            {
                var authHeader = httpContext.Request.Headers["Authorization"];
                if (authHeader == null)
                    throw new AuthenticationException();

                var token = authHeader.ToString();

                JwtBuilder
                        .Create()
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(secretKey)
                        .MustVerifySignature()
                        .Decode(token);

                return true;
            }
            catch
            {
                return false;
            }
        }

        //     Обрабатывает HTTP-запрос, не прошедший авторизацию.
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }

        //     Вызывается, когда модуль кэширования запрашивает авторизацию.
        // Возврат:
        //     Ссылка на состояние проверки.
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }
    }
}