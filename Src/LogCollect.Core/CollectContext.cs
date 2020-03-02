using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public class CollectContext : ICollectContext
    {
        public ICollectContextFields Fields { get; private set; }

        public CollectContext()
        {
            Fields = new JsonCollectContextFields();
        }
    }

    internal class JsonCollectContextFields : ICollectContextFields
    {
        JObject _root;

        public JsonCollectContextFields()
        {
            _root = new JObject();
        }

        public string this[string key]
        {
            get
            {
                if (_root.ContainsKey(key))
                {
                    return _root[key].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _root[key] = value;
            }
        }

        public bool Remove(string key)
        {
            if (_root.ContainsKey(key))
            {
                return _root.Remove(key);
            }
            else
            {
                return true;
            }
        }

        public override string ToString()
        {
            return _root.ToString();
        }
    }
}
