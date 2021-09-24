using System.Web.Mvc;
using AnResh.Models;
using Dapper;

namespace AnResh.Controllers
{
    public class DepartmentsController : Controller
    {
        private DepartmentRepository _repository = new DepartmentRepository();

        public ActionResult Index()
        {
            return View(_repository.GetDepartments());
        }

        public ActionResult Create()
        {
            var department = new Department();
            return View(department);
        }

        [HttpPost]
        public ActionResult Create(Department department)
        {
            var param = new DynamicParameters();
            param.Add("DepartmentName", department.DepartmentName);
            param.Add("DepartmentId", department.DepartmentId);
            DapperORM.Create<Department>(param, department);
            return RedirectToAction("Index");
        }
    }
}