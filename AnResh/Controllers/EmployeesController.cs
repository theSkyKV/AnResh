using System;
using System.Web.Mvc;
using AnResh.Attributes;
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
            var employees = _repository.GetAll(page, out int totalPages);
            var response = new { employees = employees, totalPages = totalPages };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllByDepartmentId(int id, PageViewModel page)
        {
            var employees = _repository.GetAllByDepartmentId(id, page, out int totalPages);
            var response = new { employees = employees, totalPages = totalPages };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            var employee = _repository.GetById(id);
            var response = new { employee = employee };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var response = new { };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [CustomAuth]
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                _repository.Create(employee);
                var response = new { };
                return Json(response);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult Edit(int id)
        {
            return GetById(id);
        }

        [CustomAuth]
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                _repository.Edit(employee);
                var response = new { };
                return Json(response);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult Delete(int id)
        {
            return GetById(id);
        }

        [CustomAuth]
        [HttpPost]
        public ActionResult Delete(Employee employee)
        {
            try
            {
                _repository.Delete(employee);
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