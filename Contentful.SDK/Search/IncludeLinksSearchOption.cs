using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contentful.SDK.Search
{
    public class IncludeLinksSearchOption : SearchFilter
    {
        public IncludeLinksSearchOption(int level)
        {
            if (level <= 0) throw new ArgumentOutOfRangeException();
            Set("include", level.ToString());
        }
    }
}
