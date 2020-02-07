using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.TidyFields
{
    public class TidyFieldsFilter : BaseCollectFilter
    {
        public const string CCode = "tidy-fields";

        List<TidyOptions> _tidyFields;

        public override string Code => CCode;

        public TidyFieldsFilter(
            ILogger<TidyFieldsFilter> logger
            ) : base(logger)
        {

        }

        public override bool Init(ICollectFilterOptions options)
        {
            _tidyFields = new List<TidyOptions>();

            var json = options.ToString();

            var root = JObject.Parse(json);

            var p = root.First;
            while (p != null)
            {
                foreach (JProperty c in root[p.Path].Children())
                {
                    var fo = new TidyOptions();
                    fo.Type = p.Path;
                    fo.DstField = c.Name;
                    fo.SrcField = c.Value.ToString();

                    _tidyFields.Add(fo);
                }

                p = p.Next;
            }

            return true;
        }

        public override bool Input(ICollectContext context)
        {
            foreach (var to in _tidyFields)
            {
                if (to.Type == "combine")
                {
                    CombineField(context, to);
                }
                else if (to.Type == "rename")
                {
                    RenameField(context, to);
                }
            }

            return OutputContext(context);
        }

        private void CombineField(ICollectContext context, TidyOptions options)
        {
            var srcFields = options.SrcField.Split(' ');
            var dstValue = "";

            foreach (var sf in srcFields)
            {
                var src = context.Fields[sf];

                if (!string.IsNullOrEmpty(src))
                {
                    dstValue += $"{src}_";
                }
            }

            if (dstValue.Length > 1)
            {
                dstValue = dstValue.Remove(dstValue.Length - 1, 1);
            }

            context.Fields[options.DstField] = dstValue;
        }

        private void RenameField(ICollectContext context, TidyOptions options)
        {
            context.Fields[options.SrcField] = context.Fields[options.DstField];

            context.Fields.Remove(options.DstField);
        }
    }
}
