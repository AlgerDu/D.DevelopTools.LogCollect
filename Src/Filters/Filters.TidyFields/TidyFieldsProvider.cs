using D.DevelopTools.LogCollect.Filters.TidyFields;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters
{
    public class TidyFieldsProvider : ICollectFilterProvider
    {
        public IDictionary<string, Type> Get()
        {
            return new Dictionary<string, Type>
            {
                {TidyFieldsFilter.CCode,typeof(TidyFieldsFilter) }
            };
        }
    }
}
