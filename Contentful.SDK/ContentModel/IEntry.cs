using Newtonsoft.Json.Linq;

namespace Contentful.SDK.ContentModel
{
    public interface IEntry : IContent
    {
        JObject Fields { get; set; }
        T GetField<T>(string propertyName);
    }
}