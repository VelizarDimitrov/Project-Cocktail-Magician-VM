﻿using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CocktailMagician.Infrastructure.Middleware
{
    public class WrongRouteMiddleware
    {
        private readonly RequestDelegate next;

        public WrongRouteMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);

                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/404");
                }
            }
            catch (Exception ex)
            {
                context.Response.Redirect("/errorPage");
            }
        }
    }
}
