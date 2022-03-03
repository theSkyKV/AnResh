using AnResh.Attributes;
using AnResh.Repositories;
using System;
using System.Web.Mvc;

namespace AnResh.Controllers
{
    public class SkillbookController : Controller
    {
        private SkillbookRepository _repository = new SkillbookRepository();

        public ActionResult GetAllForOne(int id)
        {
            try
            {
                var skills = _repository.GetAllForOne(id);
                var response = new { skills = skills };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult GetAllForMany(int[] ids)
        {
            try
            {
                var skills = _repository.GetAllForMany(ids);
                var response = new { skills = skills };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        [CustomAuth]
        [HttpPost]
        public ActionResult Update(int employeeId, int[] skillIds)
        {
            try
            {
                _repository.Update(employeeId, skillIds);
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