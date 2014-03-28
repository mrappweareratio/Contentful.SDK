namespace Contentful.SDK.Search
{
    public class EqualitySearchFilter : SearchFilter
    {
        public EqualitySearchFilter(string propertyPath, string propertyValue )
        {
            Set(propertyPath, propertyValue);
        }
    }
}