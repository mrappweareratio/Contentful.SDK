namespace Contentful.SDK.Configuration
{
    public interface IContentfulConfiguration
    {
        string Host { get; }
        bool Secute { get; set; }
    }
}