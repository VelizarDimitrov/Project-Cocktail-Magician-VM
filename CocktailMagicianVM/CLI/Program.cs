using Autofac;
using CLI.Core.AutofacConfig;
using CLI.Core.Contracts;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ConfigureContainer.Setup();
            var engine = container.Resolve<IEngine>();
            engine.Run();
        }
    }
}
