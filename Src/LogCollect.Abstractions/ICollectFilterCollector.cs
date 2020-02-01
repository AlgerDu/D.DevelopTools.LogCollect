using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public interface ICollectFilterProvider
    {
        IDictionary<string, Type> Get();
    }
}
