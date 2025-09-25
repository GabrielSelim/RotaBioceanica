using Projeto_Gabriel.Application.Hypermedia.Abstract;

namespace Projeto_Gabriel.Application.Hypermedia.Utils
{
    public class PagedSearchDbo<T> where T : ISupportHyperMedia
    {
        public PagedSearchDbo() { }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalResults { get; set; } = 0;
        public string SortFields { get; set; } = string.Empty;
        public string SortDirections { get; set; } = string.Empty;
        public Dictionary<string, object> Filters { get; set; } = new Dictionary<string, object>();
        public List<T> List { get; set; } = new List<T>();

        public PagedSearchDbo(int currentPage, int pageSize, int totalResults, string sortFields, string sortDirections)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalResults = totalResults;
            SortFields = sortFields;
            SortDirections = sortDirections;
        }

        public PagedSearchDbo(int currentPage, int pageSize, string sortFields, string sortDirections, Dictionary<string, object> filters)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortFields = sortFields;
            SortDirections= sortDirections;
            Filters = filters;
        }
        public int GetCurrentPage()
        {
            return CurrentPage == 0 ? 2 : CurrentPage;
        }
        public int GetPageSize()
        {
            return PageSize == 0 ? 10 : PageSize;
        }

    }
}
