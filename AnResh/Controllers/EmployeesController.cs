using System.Web.Mvc;
using AnResh.Models;
using AnResh.Repositories;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeRepository _repository = new EmployeeRepository();

        public ActionResult GetAll(PageViewModel page)
        {
            var employees = _repository.GetAll(page, out TotalViewModel total);
            var response = new { employees = employees, total = total };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByDepartmentId(int id, PageViewModel page)
        {
            var employees = _repository.GetAllByDepartmentId(id, page, out TotalViewModel total);
            var response = new { employees = employees, total = total };
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
        public ActionResult Edit(EmployeeEditViewModel employee)
        {
            _repository.Edit(employee);
            var response = new { };
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