using AnResh.Enums;

namespace AnResh.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int Limit { get; set; }
        public string SearchQuery { get; set; } = "";
        public string SearchNameQuery { get; set; } = "";
        public string SearchDepartmentQuery { get; set; } = "";
        public int[] SearchedSkills { get; set; }
        public SortingOption SelectedSort { get; set; } = SortingOption.ByName;
    }
}