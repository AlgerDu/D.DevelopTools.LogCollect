using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect.Filters.DRegex
{
    internal class RegexFilter : BaseCollectFilter, ICollectFilter
    {
        public const string CCode = "regex";

        public override string Code => CCode;

        public RegexFilter(
            ILogger<RegexFilter> logger
            ) : base(logger)
        {

        }
    }
}
