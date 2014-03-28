using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contentful.SDK.ContentModel
{
    public class LinkedContentAttribute : Attribute
    {

        public LinkedContentAttribute(Type targetType, LinkType linkType)
        {
            TargetType = targetType;
            LinkType = linkType;
        }

        public Type TargetType { get; set; }
        public LinkType LinkType { get;set; }
    }

    public class LinkedContentArrayAttribute : Attribute
    {
        public LinkedContentArrayAttribute(Type targetType, LinkType linkType)
        {
            TargetType = targetType;
            LinkType = linkType;
        }
        public Type TargetType { get; set; }
        public LinkType LinkType { get; set; }
    }
}
