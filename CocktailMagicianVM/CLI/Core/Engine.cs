using CLI.Core.Contracts;
using ServiceLayer.Contracts;
using System;

namespace CLI.Core
{
    class Engine : IEngine
    {
        private readonly IAccountService aService;
        private readonly ICocktailService cService;
        private readonly IBarService bService;

        public Engine(IAccountService aService, ICocktailService cService, IBarService bService)
        {
            this.aService = aService;
            this.cService = cService;
            this.bService = bService;
        }

        public void Run()
        {
            aService.DatabaseUserFill();
            cService.DatabaseCocktailFill();
            bService.DatabaseBarFill();
            
        }
    }
}
