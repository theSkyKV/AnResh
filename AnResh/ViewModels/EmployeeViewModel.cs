using System.Collections.Generic;

namespace AnResh.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int Salary { get; set; }
        public string DepartmentName { get; set; }
        public List<LearnedSkillViewModel> Skills { get; set; }
    }
}