using AnResh.Models;
using AnResh.AuthenticationServer;
using System;
using System.Web.Mvc;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class AuthController : Controller
    {
        private AuthServer _authServer = new AuthServer();

        [HttpPost]
        public ActionResult SignIn(SignInViewModel auth)
        {
            try
            {
                var token = _authServer.SignIn(auth);
                var response = new { accessToken = token };
                return Json(response);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            try
            {
                _authServer.SignUp(user);
                var response = new { };
                return Json(response);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }
    }
}