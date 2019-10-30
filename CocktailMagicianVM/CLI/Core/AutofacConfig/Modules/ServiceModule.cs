using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLI.Core.AutofacConfig.Modules
{
    class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountsService>().As<IAccountsService>().SingleInstance();
            builder.RegisterType<CocktailService>().As<ICocktailService>().SingleInstance();
            builder.RegisterType<BarService>().As<IBarService>().SingleInstance();
            builder.RegisterType<CocktailMagicianDatabaseContext>().AsSelf().SingleInstance();
            builder.RegisterType<Hashing>().As<IHashing>().SingleInstance();

        }
    }
}
