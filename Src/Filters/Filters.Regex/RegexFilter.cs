using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.DRegex
{
    internal class RegexFilter : BaseCollectFilter, ICollectFilter
    {
        public const string CCode = "regex";

        List<FieldOptions> _fieldOptions;

        public override string Code => CCode;

        public RegexFilter(
            ILogger<RegexFilter> logger
            ) : base(logger)
        {

        }

        public override bool Init(ICollectFilterOptions options)
        {
            _fieldOptions = new List<FieldOptions>();

            var json = options.ToString();

            var root = JObject.Parse(json);

            var p = root.First;
            while (p != null)
            {
                foreach (JProperty c in root[p.Path].Children())
                {
                    var fo = new FieldOptions();
                    fo.SrcField = p.Path;
                    fo.DstField = c.Name;
                    fo.Pattern = c.Value.ToString();

                    _fieldOptions.Add(fo);
                }

                p = p.Next;
            }

            return true;
        }

        public override Task Input(ICollectContext context)
        {
            return Task.Run(() =>
            {
                foreach (var o in _fieldOptions)
                {
                    var src = context.Fields[o.SrcField];

                    if (string.IsNullOrEmpty(src))
                    {
                        context.Fields[o.DstField] = "";
                    }
                    else
                    {
                        var match = Regex.Match(src, o.Pattern);

                        if (match.Success)
                        {
                            var gs = match.Groups;

                            context.Fields[o.DstField] = gs[gs.Count - 1].Value;
                        }
                        else
                        {
                            context.Fields[o.DstField] = "";
                        }
                    }
                }

                _output(context);
            });
        }
    }
}
