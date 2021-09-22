using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class DepartmentsController : Controller
    {
        public ActionResult Index()
        {
            return View(DapperORM.GetAllElementsByType<Department>());
        }
    }
}