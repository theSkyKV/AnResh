﻿using System.Web.Mvc;
using AnResh.Models;
using AnResh.Repositories;

namespace AnResh.Controllers
{
    public class DepartmentsController : Controller
    {
        private DepartmentRepository _repository = new DepartmentRepository();

        public ActionResult GetAll()
        {
            var departments = _repository.GetAll();
            var response = new { departments = departments };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            var department = _repository.GetById(id);
            var response = new { department = department };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Department department)
        {
            _repository.Create(department);
            var response = new { };
            return Json(response);
        }

        public ActionResult Edit(int id)
        {
            return GetById(id);
        }

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            _repository.Edit(department);
            var response = new { };
            return Json(response);
        }

        public ActionResult Delete(int id)
        {
            return GetById(id);
        }

        [HttpPost]
        public ActionResult Delete(Department department)
        {
            _repository.Delete(department);
            var response = new { };
            return Json(response);
        }
    }
}