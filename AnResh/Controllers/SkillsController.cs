using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AnResh.Attributes;
using AnResh.AuthenticationServer;
using AnResh.Models;
using AnResh.Repositories;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class SkillsController : Controller
    {
        private SkillRepository _repository = new SkillRepository();

        [CustomAuth]
        public ActionResult GetAll(PageViewModel page)
        {
            //var authServer = new AuthServer();
            //var check = authServer.CheckToken(HttpContext, out Dictionary<string, object> payload);

            //if (check == false)
            //    return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError), JsonRequestBehavior.AllowGet);

            var skills = _repository.GetAll(page, out TotalViewModel total);
            var response = new { skills = skills, total = total };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            var skill = _repository.GetById(id);
            var response = new { skill = skill };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Skill skill)
        {
            _repository.Create(skill);
            var response = new { };
            return Json(response);
        }

        public ActionResult Edit(int id)
        {
            return GetById(id);
        }

        [HttpPost]
        public ActionResult Edit(Skill skill)
        {
            _repository.Edit(skill);
            var response = new { };
            return Json(response);
        }

        public ActionResult Delete(int id)
        {
            return GetById(id);
        }

        [HttpPost]
        public ActionResult Delete(Skill skill)
        {
            _repository.Delete(skill);
            var response = new { };
            return Json(response);
        }
    }
}