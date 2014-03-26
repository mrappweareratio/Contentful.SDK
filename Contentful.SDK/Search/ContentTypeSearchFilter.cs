namespace Contentful.SDK.Search
{
    public class ContentTypeSearchFilter : SearchFilter
    {
        public string ContentType { get { return Get("content_type"); } set { Set("content_type", value); } }
    }
}