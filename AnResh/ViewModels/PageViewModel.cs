using AnResh.Enums;

namespace AnResh.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int Limit { get; set; }
        public string SearchQuery { get; set; }
        public SortingOption SelectedSort { get; set; } = SortingOption.ByName;
    }
}