using System.Collections.Generic;
using System.Web.Mvc;
using AnResh.Models;
using AnResh.ViewModels;

namespace AnResh.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeRepository _repository = new EmployeeRepository();

        public ActionResult Index()
        {
            return RedirectToAction("ViewAll");
        }

        public ActionResult ViewAll()
        {
            return View(_repository.GetAllEmployeesWithViewModel());
        }

        public ActionResult Create(int departmentId = 0)
        {
            var employee = new Employee() { DepartmentId = departmentId };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            _repository.Create(employee);
            return RedirectToAction("ViewById", "Departments", new { id = employee.DepartmentId });
        }

        public ActionResult Edit(int id, string name, int departmentId, int salary)//только id
        {
            var skillRepository = new SkillRepository();
            var departmentRepository = new DepartmentRepository();

            List<string> learnedSkillNames;

            var skills = skillRepository.GetSkillsWithFlag(id, out learnedSkillNames);
            var departments = departmentRepository.GetDepartments();
            var employee = new EditingEmployeeViewModel()
            {
                EmployeeId = id,
                EmployeeName = name,
                DepartmentId = departmentId,
                Salary = salary,
                Departments = departments,
                Skills = skills
            };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(int employeeId, string employeeName, int departmentId, int salary, int[] skills)
        {
            _repository.Edit(employeeId, employeeName, departmentId, salary, skills);
            return RedirectToAction("ViewById", "Departments", new { id = departmentId });
        }

        public ActionResult Delete(int id, string name, int departmentId, int salary)
        {
            var employee = new Employee() { EmployeeId = id, EmployeeName = name, DepartmentId = departmentId, Salary = salary };
            return View(employee);
        }

        [HttpPost]
        public ActionResult Delete(int id, int departmentId)
        {
            _repository.Delete(id);
            return RedirectToAction("ViewById", "Departments", new { id = departmentId });
        }
    }
}