using CocktailMagician.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace CocktailMagician.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseWrongRouteHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<WrongRouteMiddleware>();
        }
    }
}
