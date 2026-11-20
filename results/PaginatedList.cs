namespace E_commerce_System.results
{
    public class PaginatedList<T>
    {
        private const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int TotalRecords { get; set; }
        public int TotalPage { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > maxPageSize ? maxPageSize : value;
        }

        public IEnumerable<T>? Data { get; set; }

    }
}
