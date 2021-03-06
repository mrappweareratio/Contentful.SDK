namespace Contentful.SDK.Search
{
    public class PaginationSearchOption : SearchFilter
    {
        public PaginationSearchOption(int? skip = null, int? limit = null)
        {
            Skip = skip;
            Limit = limit;
        }

        public int? Skip
        {
            get { return Get("skip") == null ? (int?)null : int.Parse(Get("skip")); }
            set { Set("skip", value == null ? null : value.ToString()); }
        }
        public int? Limit
        {
            get { return Get("limit") == null ? (int?)null : int.Parse(Get("limit")); }
            set { Set("limit", value == null ? null : value.ToString()); }
        }
    }
}