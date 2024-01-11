namespace DataAccess.DTO
{
    public class PagedResultDTO<T> where T : class
    {
        public PagedResultDTO()
        {
            Results = new List<T>();
        }
        public int PageSelected { get; set; }
        public List<T> Results { get; set; }
        public int NumberPage { get; set; }
        public string PRE_URL { get; set; } = null!;
        public string NEXT_URL { get; set; } = null!;

    }
}
