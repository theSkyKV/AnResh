using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class SkillsController : Controller
    {
        private SkillRepository _repository = new SkillRepository();

        public ActionResult ViewAll()
        {
            return View(_repository.GetSkills());
        }

        public ActionResult Create()
        {
            var skill = new Skill();
            return View(skill);
        }

        [HttpPost]
        public ActionResult Create(Skill skill)
        {
            _repository.Create(skill);
            return RedirectToAction("ViewAll");
        }

        public ActionResult Delete(int id, string name)
        {
            var skill = new Skill() { SkillId = id, SkillName = name };
            return View(skill);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
             _repository.Delete(id);

            return RedirectToAction("ViewAll");
        }
    }
}