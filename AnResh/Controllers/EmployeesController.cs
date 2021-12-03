using System.Web.Mvc;
using AnResh.Models;
using AnResh.Repositories;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeRepository _repository = new EmployeeRepository();

        public ActionResult GetAll()
        {
            var employees = _repository.GetAll();
            var response = new { employees = employees };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByDepartmentId(int id)
        {
            var employees = _repository.GetAllByDepartmentId(id);
            var response = new { employees = employees };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            var employee = _repository.GetById(id);
            var response = new { employee = employee };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            _repository.Create(employee);
            var response = new{ };
            return Json(response);
        }

        public ActionResult Edit(int id)
        {
            return GetById(id);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            _repository.Edit(employee);
            var response = new{ };
            return Json(response);
        }

        public ActionResult Delete(int id)
        {
            return GetById(id);
        }

        [HttpPost]
        public ActionResult Delete(Employee employee)
        {
            _repository.Delete(employee);
            var response = new { };
            return Json(response);
        }
    }
}