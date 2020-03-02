using D.DevelopTools.LogCollect.Filters.DRegex;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters
{
    public class RegexFilterProvider : ICollectFilterProvider
    {
        public IDictionary<string, Type> Get()
        {
            return new Dictionary<string, Type>
            {
                {RegexFilter.CCode,typeof(RegexFilter) }
            };
        }
    }
}
