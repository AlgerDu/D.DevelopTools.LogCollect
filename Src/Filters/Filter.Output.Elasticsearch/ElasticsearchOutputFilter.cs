using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Output.Elasticsearch
{
    internal class ElasticsearchOutputFilter : BaseCollectFilter
    {
        public const string CCode = "elasticsearch-output";

        ElasticsearchOutputFilterOptions _options;
        HttpClient _http;

        public override string Code => CCode;

        public ElasticsearchOutputFilter(
            ILogger<ElasticsearchOutputFilter> logger
            ) : base(logger)
        {
            _http = new HttpClient();
        }

        public override bool Init(ICollectFilterOptions options)
        {
            _options = options.ConvertTo<ElasticsearchOutputFilterOptions>();

            return true;
        }

        public override Task Input(ICollectContext context)
        {
            return Task.Run(() =>
            {
                var body = new JObject();

                foreach (var f in _options.Fields)
                {
                    body[f] = context.Fields[f];
                }

                var index = context.Fields[_options.Index];
                var type = context.Fields[_options.Type].Replace('.','_');
                var url = $"http://{_options.Host}/logcollect_{index}/{type}";

                var content = new StringContent(body.ToString());
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                try
                {
                    _http.PostAsync(url, content).ContinueWith((t) =>
                    {
                        _logger.LogInformation($"{index} {t.Result.StatusCode}");
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{ex}");
                }
            });
        }
    }
}
