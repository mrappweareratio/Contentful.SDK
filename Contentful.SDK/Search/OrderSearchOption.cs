using System;

namespace Contentful.SDK.Search
{
    public class OrderSearchOption : SearchFilter
    {
        public string Order { get { return Get("order"); } set { Set("order", value); } }

        public bool Descending { get; set; }

        protected override string GetQueryString()
        {
            return String.Format("order={0}", Descending ? "-" + Order : Order);
        }
    }
}