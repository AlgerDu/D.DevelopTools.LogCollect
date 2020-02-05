using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.TidyFields
{
    public class TidyFieldsFilter : BaseCollectFilter
    {
        public const string CCode = "tidy-fields";

        public override string Code => CCode;

        public TidyFieldsFilter(
            ILogger<TidyFieldsFilter> logger
            ) : base(logger)
        {

        }
    }
}
