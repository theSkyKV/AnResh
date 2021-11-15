using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnResh.ViewModels
{
    public class EmployeeViewModel
    {
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public string DepartmentName { get; set; }
        public int Salary { get; set; }
        public List<SkillViewModel> Skills { get; set; }
        public List<string> LearnedSkills { get; set; }
    }
}