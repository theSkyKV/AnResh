namespace AnResh.ViewModels
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int Salary { get; set; }
        public int[] Skills { get; set; }
    }
}