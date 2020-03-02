using D.DevelopTools.LogCollect.Filters.Input.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters
{
    public class FileInputCollectFilterProvider : ICollectFilterProvider
    {
        public IDictionary<string, Type> Get()
        {
            return new Dictionary<string, Type>
            {
                {FileInputFilter.CCode,typeof(FileInputFilter) }
            };
        }
    }
}
