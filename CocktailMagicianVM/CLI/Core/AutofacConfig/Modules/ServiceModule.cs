using Autofac;
using ServiceLayer;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Data;

namespace CLI.Core.AutofacConfig.Modules
{
    class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<CocktailService>().As<ICocktailService>().SingleInstance();
            builder.RegisterType<BarService>().As<IBarService>().SingleInstance();
            builder.RegisterType<CocktailDatabaseContext>().AsSelf().SingleInstance();
            builder.RegisterType<Hashing>().As<IHashing>().SingleInstance();

        }
    }
}
