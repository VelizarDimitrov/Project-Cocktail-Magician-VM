using Data;
using Data.SolutionPreLoad.JsonParsers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task DatabaseCocktailFillAsync()
        {
            string drinksToRead = File.ReadAllText(@"../../../../Data/SolutionPreload/Cocktails.json");
            List<CocktailJson> listOfDrinks = JsonConvert.DeserializeObject<List<CocktailJson>>(drinksToRead);

            foreach (var item in listOfDrinks)
            {
                item.PhotoPath = @"../../../../Data/SolutionPreload/CocktailPhotos/"+String.Join('-',item.Name.Split(" ")).ToLower();
            }

            if (dbContext.Cocktail.Count() == 0)
            {
                foreach (var item in listOfDrinks)
                {
                    var photo = File.ReadAllBytes(item.PhotoPath);
                    await CreateDrinkAsync(item.Name, item.Description, item.Ingredient, photo);
                }
            }
        }

        public async Task CreateDrinkAsync(string name, string description, string[] ingredients, byte[] photo)
        {
            var cocktail = new Cocktail()
            {
                Name = name,
                Description = description,
                Photo = photo
            };
            await dbContext.Cocktail.AddAsync(cocktail);
            await dbContext.SaveChangesAsync();

            var ingCounter = 0;
            foreach (var item in ingredients)
            {
                if (await dbContext.Ingredient.Where(p => p.Name == item.ToLower()).CountAsync() == 0)
                {
                    if (ingCounter == 0)
                        await CreateIngredientAsync(item, 1);
                    else
                        await CreateIngredientAsync(item, 0);
                }
                ingCounter++;
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower());
            }
        }

        public async Task AddIngredientToCocktailAsync(string cocktailName, string ingredientName)
        {
            var cocktail = await dbContext.Cocktail.FirstAsync(p=>p.Name==cocktailName);
            var ingredient = await dbContext.Ingredient.FirstAsync(p => p.Name == ingredientName);
            var link = new CocktailIngredient()
            {
                Cocktail = cocktail,
                Ingredient = ingredient
            };
            await dbContext.CocktailIngredient.AddAsync(link);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateIngredientAsync(string name, byte primary )
        {
            var ingredient = new Ingredient()
            {
                Name = name.ToLower(),
                Primary = primary
            };

            await dbContext.Ingredient.AddAsync(ingredient);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Ingredient> GetIngredientAsync(string name)
        {
            var ingredient = await dbContext.Ingredient.Where(p => p.Name == name.ToLower()).FirstAsync();
            return ingredient;
        }
    }
}
