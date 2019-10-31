using Autofac;
using CLI.Core.AutofacConfig.Modules;

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
