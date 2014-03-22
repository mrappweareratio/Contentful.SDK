namespace Contentful.SDK.Components
{
    public interface IContentful
    {
        IContentfulClient CreateClient(string accessToken, string space);
    }
}