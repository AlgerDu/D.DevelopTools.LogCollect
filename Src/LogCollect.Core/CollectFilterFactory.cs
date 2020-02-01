using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public class CollectFilterFactory : ICollectFilterFactory
    {
        ILogger _logger;

        IEnumerable<ICollectFilterProvider> _providers;
        ILifetimeScope _lifetimeScope;

        IDictionary<string, Type> _filterTypes;

        public CollectFilterFactory(
            ILogger<CollectFilterFactory> logger
            , IEnumerable<ICollectFilterProvider> providers
            , ILifetimeScope lifetimeScope
            )
        {
            _logger = logger;
            _providers = providers;
            _lifetimeScope = lifetimeScope;

            _filterTypes = new Dictionary<string, Type>();

            foreach (var p in _providers)
            {
                foreach (var t in p.Get())
                {
                    _filterTypes.Add(t);
                }
            }
        }

        public ICollectFilter Create(string code)
        {
            if (!_filterTypes.ContainsKey(code))
            {
                _logger.LogWarning($"不支持的 {code} filter");
                return null;
            }

            var t = _filterTypes[code];

            using (var scope = _lifetimeScope.BeginLifetimeScope((builder) =>
             {
                 builder.RegisterType(t);
             }))
            {
                return scope.Resolve(t) as ICollectFilter;
            }
        }
    }
}
