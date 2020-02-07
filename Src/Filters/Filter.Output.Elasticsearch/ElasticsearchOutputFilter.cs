using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D.DevelopTools.LogCollect.Filters.Output.Elasticsearch
{
    internal class ElasticsearchOutputFilter : BaseCollectFilter
    {
        public const string CCode = "elasticsearch-output";

        ElasticsearchOutputFilterOptions _options;
        HttpClient _http;

        Queue<ICollectContext> _queue;

        ManualResetEvent mre_addContext;

        bool _isRunning;

        public override string Code => CCode;

        public ElasticsearchOutputFilter(
            ILogger<ElasticsearchOutputFilter> logger
            ) : base(logger)
        {
            _http = new HttpClient();

            _queue = new Queue<ICollectContext>(500);
            mre_addContext = new ManualResetEvent(false);
        }

        public override bool Init(ICollectFilterOptions options)
        {
            _options = options.ConvertTo<ElasticsearchOutputFilterOptions>();

            return true;
        }

        protected override bool StopToRun()
        {
            StartSendingThread();
            return true;
        }

        protected override bool PauseToRun()
        {
            StartSendingThread();
            return true;
        }

        protected override bool RunToPause()
        {
            mre_addContext.WaitOne();
            return true;
        }

        protected override bool TryToStop()
        {
            mre_addContext.WaitOne();

            return true;
        }

        public override bool Input(ICollectContext context)
        {
            lock (this)
            {
                if (_queue.Count == 500)
                {
                    return false;
                }

                _queue.Enqueue(context);
                mre_addContext.Set();

                return true;
            }
        }

        private void StartSendingThread()
        {
            Task.Run(() =>
            {
                while (State == FilterState.Running)
                {
                    ICollectContext context = null;

                    lock (this)
                    {
                        if (_queue.Count > 0)
                            context = _queue.Dequeue();

                        if (_queue.Count < 200)
                        {
                            Task.Run(() => this.NotiifyPipelineEmpty());
                        }
                    }

                    if (context != null)
                    {
                        SendContext(context);
                    }

                    if (context == null)
                    {
                        mre_addContext.WaitOne();
                    }
                }
            });
        }

        private void SendContext(ICollectContext context)
        {
            var body = new JObject();

            foreach (var f in _options.Fields)
            {
                body[f] = context.Fields[f];
            }

            var index = context.Fields[_options.Index];
            var type = context.Fields[_options.Type].Replace('.', '_');
            var url = $"http://{_options.Host}/logcollect_{index}/{type}";

            var content = new StringContent(body.ToString());
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            try
            {
                _http.PostAsync(url, content).ContinueWith((t) =>
                {
                    var rsp = t.Result;

                    _logger.LogInformation($"{index} {(int)rsp.StatusCode}");

                    _logger.LogDebug($"{rsp.RequestMessage.Headers.ToString()}");
                    _logger.LogDebug($"{rsp.Headers.ToString()}");
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{ex}");
            }
        }
    }
}
