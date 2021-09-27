using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeRepository _repository = new EmployeeRepository();

        public ActionResult Index(int departmentId = 0, string departmentName = "")
        {
            ViewBag.DepartmentId = departmentId;
            ViewBag.DepartmentName = departmentName;
            return View(_repository.GetEmployeesByDepartmentId(departmentId));
        }

        public ActionResult Create(int departmentId)
        {
            var employee = new Employee() { DepartmentId = departmentId};
            return View(employee);
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            _repository.Create(employee);
            return RedirectToAction("Index", "Departments");
        }

        public ActionResult Edit(int employeeId, string employeeName, int departmentId, int salary)
        {
            var employee = new Employee() { EmployeeId = employeeId, EmployeeName = employeeName, DepartmentId = departmentId, Salary = salary };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            _repository.Edit(employee);
            return RedirectToAction("Index", "Departments");
        }

        public ActionResult Delete(int employeeId, string employeeName, int departmentId, int salary)
        {
            var employee = new Employee() { EmployeeId = employeeId, EmployeeName = employeeName, DepartmentId = departmentId, Salary = salary };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Delete(int employeeId)
        {
            _repository.Delete(employeeId);
            return RedirectToAction("Index", "Departments");
        }
    }
}