using Autofac;
using ServiceLayer;
using ServiceLayer.Contracts;
using Data;

namespace CLI.Core.AutofacConfig.Modules
{
    class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<CocktailService>().As<ICocktailService>().SingleInstance();
            builder.RegisterType<CountryService>().As<ICountryService>().SingleInstance();
            builder.RegisterType<CityService>().As<ICityService>().SingleInstance();
            builder.RegisterType<BarService>().As<IBarService>().SingleInstance();
            builder.RegisterType<CocktailDatabaseContext>().AsSelf().SingleInstance();
            builder.RegisterType<Hashing>().As<IHashing>().SingleInstance();
            builder.RegisterType<IngredientService>().As<IIngredientService>().SingleInstance();
            builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
        }
    }
}
