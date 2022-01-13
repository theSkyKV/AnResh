using AnResh.Models;
using AnResh.Repositories;
using AnResh.AuthenticationServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class AuthController : Controller
    {
        private AuthRepository _repository = new AuthRepository();
        private AuthServer _authServer = new AuthServer();

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            _repository.SignUp(user);
            var response = new { };
            return Json(response);
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel user)
        {
            var token = _authServer.Auth(user, out Dictionary<string, object> payload);
            var response = new { accessToken = token, payload = payload };
            return Json(response);
        }
    }
}