using System.Web.Mvc;
using AnResh.Models;

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
            _repository.Create(department);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string name)
        {
            var department = new Department() { DepartmentId = id, DepartmentName = name };
            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            _repository.Edit(department);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id, string name)
        {
            var department = new Department() { DepartmentId = id, DepartmentName = name };
            return View(department);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }
    }
}