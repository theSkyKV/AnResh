using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        public ActionResult Index(int id = 0, string departmentName = "")
        {
            ViewBag.DepartmentName = departmentName;
            return View(DapperORM.GetAllElementsFromById<Employee, Department>(id));
        }
    }
}