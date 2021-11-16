using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class DepartmentsController : Controller
    {
        private DepartmentRepository _repository = new DepartmentRepository();

        public ActionResult Index(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("ViewAll");

            return RedirectToAction("ViewById", new { id });
        }

        public ActionResult ViewAll()
        {
            return View(_repository.GetDepartments());
        }

        public ActionResult ViewById(int id)
        {
            return View(_repository.GetDepartmentWithViewModel(id));
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
            return RedirectToAction("ViewAll");
        }

        //зачем передавать название? нужно поднимать по ИД из БД
        public ActionResult Edit(int id, string name)
        {
            var department = new Department() { DepartmentId = id, DepartmentName = name };
            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(Department department)
        {
            _repository.Edit(department);
            return RedirectToAction("ViewAll");
        }

        //зачем передавать название? нужно поднимать по ИД из БД
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
                return View("DeleteError");
            }

            return RedirectToAction("ViewAll");
        }
    }
}