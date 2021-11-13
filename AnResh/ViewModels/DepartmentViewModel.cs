using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnResh.ViewModels
{
    public class DepartmentViewModel
    {
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
    }
}