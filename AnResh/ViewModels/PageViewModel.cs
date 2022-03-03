namespace AnResh.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int Limit { get; set; }
        public string OrderBy { get; set; } = "Name";
        public string SearchName { get; set; } = "";
        public string SearchDepartment { get; set; } = "";
        public int[] SearchSkills { get; set; }
    }
}