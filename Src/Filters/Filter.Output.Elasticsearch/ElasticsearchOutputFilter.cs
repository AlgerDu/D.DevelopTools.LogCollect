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
        bool _isFull;

        public override string Code => CCode;

        public ElasticsearchOutputFilter(
            ILogger<ElasticsearchOutputFilter> logger
            ) : base(logger)
        {
            _http = new HttpClient();

            _queue = new Queue<ICollectContext>(500);
            mre_addContext = new ManualResetEvent(false);

            _isFull = false;
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
                    _isFull = true;
                    _logger.LogInformation($"{this} 待发送的数据已经达到 500，管道开始堵塞");
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
                        {
                            context = _queue.Dequeue();
                            //_logger.LogTrace($"{this} 队列缓存 {_queue.Count}");
                        }

                        if (_queue.Count < 200 && _isFull)
                        {
                            _isFull = false;
                            _logger.LogInformation($"{this} 管道开始畅通");
                            Task.Run(() => this.NotiifyPipelineEmpty());
                        }
                    }

                    if (context != null)
                    {
                        //SendContext(context);
                        System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(20));
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

            if (string.IsNullOrEmpty(index))
            {
                return;
            }

            body["apptype"] = context.Fields[_options.Type];

            var url = $"http://{_options.Host}/logcollect_{index}/_doc";

            var content = new StringContent(body.ToString());
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            try
            {
                var t = _http.PostAsync(url, content);
                t.Wait();

                var rsp = t.Result;

                _logger.LogInformation($"{url} StatusCode[{(int)rsp.StatusCode}]({rsp.Headers})({content.Headers})");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{ex}");
            }
        }
    }
}
