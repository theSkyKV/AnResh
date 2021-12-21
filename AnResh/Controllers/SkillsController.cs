using System.Web.Mvc;
using AnResh.Models;
using AnResh.Repositories;
using AnResh.Enums;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class SkillsController : Controller
    {
        private SkillRepository _repository = new SkillRepository();

        //public ActionResult GetAll()
        //{
        //    var skills = _repository.GetAll();
        //    var response = new { skills = skills };
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

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