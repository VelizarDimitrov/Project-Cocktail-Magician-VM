using Data;
using Data.Models;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CocktailService : ICocktailService
    {
        private readonly CocktailDatabaseContext dbContext;
        private readonly IAccountService aService;

        public CocktailService(CocktailDatabaseContext dbContext, IAccountService aService)
        {
            this.dbContext = dbContext;
            this.aService = aService;
        }
        public async Task DatabaseCocktailFill()
        {
            string drinksToRead = File.ReadAllText(@"../../../../Data/SolutionPreload/Cocktails.json");
            List<CocktailJason> listOfDrinks = JsonConvert.DeserializeObject<List<CocktailJson>>(drinksToRead);

            if (dbContext.Cocktail.Count() == 0)
            {
                foreach (var item in listOfDrinks)
                {

                }

            }
        }

    }
}
