using AnResh.Models;
using System.Collections.Generic;

namespace AnResh.ViewModels
{
    public class CreatingEmployeeViewModel
    {
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int Salary { get; set; }
        public List<Department> Departments { get; set; }
    }
}