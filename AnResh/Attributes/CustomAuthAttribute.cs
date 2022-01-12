using AnResh.AuthenticationServer;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
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
                var token = httpContext.Request.Headers["Authorization"].ToString();

                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                decoder.Decode(token, secretKey, verify: true);
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