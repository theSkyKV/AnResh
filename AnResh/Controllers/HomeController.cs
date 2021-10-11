using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}