namespace DataAccess.Pagination
{
    public class Pagination<T> where T : class
    {
        public Pagination()
        {
            Results = new List<T>();
        }
        public int PageSelected { get; set; }
        public List<T> Results { get; set; }
        public int NumberPage { get; set; }
        public string PRE_URL { get; set; } = null!;
        public string NEXT_URL { get; set; } = null!;
        public string FIRST_URL { get; set; } = null!;
        public string LAST_URL { get; set; } = null!;

    }
}
