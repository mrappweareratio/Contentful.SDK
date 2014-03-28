namespace Contentful.SDK.Search
{
    public class ContentTypeSearchFilter : SearchFilter
    {
        public ContentTypeSearchFilter(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get { return Get("content_type"); } set { Set("content_type", value); } }
    }
}