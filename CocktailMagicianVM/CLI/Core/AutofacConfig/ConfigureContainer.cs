using Autofac;
using CLI.Core.AutofacConfig.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.AutofacConfig
{
    class ConfigureContainer
    {
        public static IContainer Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<CoreModule>();
            return builder.Build();
        }
    }
}
