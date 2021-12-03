using System.Collections.Generic;

namespace AnResh.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClanId { get; set; }
        public int Power { get; set; }
        public string ClanName { get; set; }
        public List<LearnedSkillViewModel> Skills { get; set; }
    }
}