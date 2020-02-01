using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace D.DevelopTools.LogCollect
{
    public static class Extensions_CollectCore_Autofac
    {
        public static ContainerBuilder AddLogCollectCore(this ContainerBuilder builder)
        {
            builder.RegisterType<CollectFilterFactory>()
                .As<ICollectFilterFactory>()
                .AsSelf()
                .SingleInstance();

            return builder;
        }
    }
}
