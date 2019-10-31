using Autofac;
using CLI.Core.Contracts;

namespace CLI.Core.AutofacConfig.Modules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Engine>().As<IEngine>().SingleInstance();
        }
    }
}
