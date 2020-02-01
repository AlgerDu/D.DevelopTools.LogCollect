using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    /// <summary>
    /// json 数据类型的配置
    /// </summary>
    public class JsonCollectPipelineConfig : ICollectPipelineConfig
    {
        JObject _root;

        List<ICollectFilterOptions> _filterOptions;

        public JsonCollectPipelineConfig(
            string json
            )
        {
            _root = JObject.Parse(json);

            _filterOptions = new List<ICollectFilterOptions>();

            var tmp = _root.First;
            while (tmp != null)
            {
                _filterOptions.Add(new JsonCollectFilterOptions(_root[tmp.Path], tmp.Path));

                tmp = tmp.Next;
            }
        }

        public IList<ICollectFilterOptions> FilterOptions => _filterOptions;
    }

    public class JsonCollectFilterOptions : ICollectFilterOptions
    {
        JToken _jToken;

        public JsonCollectFilterOptions(
            JToken jToken
            , string code
            )
        {
            FilterCode = code;
            _jToken = jToken;
        }

        public string FilterCode { get; private set; }

        public T ConvertTo<T>()
        {
            return _jToken.ToObject<T>();
        }
    }
}
