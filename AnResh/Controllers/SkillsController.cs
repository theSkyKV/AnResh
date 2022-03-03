using System;
using System.Web.Mvc;
using AnResh.Attributes;
using AnResh.Models;
using AnResh.Repositories;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class SkillsController : Controller
    {
        private SkillRepository _repository = new SkillRepository();

        public ActionResult GetAll(PageViewModel page)
        {
            var skills = _repository.GetAll(page, out int totalPages);
            var response = new { skills = skills, totalPages = totalPages };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            var skill = _repository.GetById(id);
            var response = new { skill = skill };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var response = new { };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [CustomAuth]
        [HttpPost]
        public ActionResult Create(Skill skill)
        {
            try
            {
                _repository.Create(skill);
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
        public ActionResult Edit(Skill skill)
        {
            try
            {
                _repository.Edit(skill);
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
        public ActionResult Delete(Skill skill)
        {
            try
            {
                _repository.Delete(skill);
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