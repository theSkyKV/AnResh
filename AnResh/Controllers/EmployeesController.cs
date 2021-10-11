using System.Web.Mvc;
using AnResh.Models;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeRepository _repository = new EmployeeRepository();

        public ActionResult ViewAll()
        {
            return View(_repository.GetAllEmployees());
        }

        public ActionResult ViewById(int departmentId = 0, string departmentName = "")
        {
            ViewBag.DepartmentId = departmentId;
            ViewBag.DepartmentName = departmentName;
            return View(_repository.GetAllEmployeesByDepartmentId(departmentId));
        }

        public ActionResult Create(int departmentId)
        {
            var employee = new Employee() { DepartmentId = departmentId };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            _repository.Create(employee);
            return RedirectToAction("ViewById", new { departmentId = employee.DepartmentId });
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
            return RedirectToAction("ViewById", new { departmentId = employee.DepartmentId });
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
            return RedirectToAction("ViewAll", "Departments");
        }
    }
}