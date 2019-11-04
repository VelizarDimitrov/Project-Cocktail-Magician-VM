using Data;
using Data.Models;
using Data.SolutionPreLoad.JsonParsers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CocktailService : ICocktailService
    {
        private readonly CocktailDatabaseContext dbContext;

        public CocktailService(CocktailDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void DatabaseCocktailFill()
        {
            string drinksToRead = File.ReadAllText(@"../../../../Data/SolutionPreload/Cocktails.json");
            List<CocktailJson> listOfDrinks = JsonConvert.DeserializeObject<List<CocktailJson>>(drinksToRead);

            foreach (var item in listOfDrinks)
            {
                item.PhotoPath = @"../../../../Data/SolutionPreload/CocktailPhotos/"+String.Join('-',item.Name.Split(" ")).ToLower()+".jpg";
            }

            if (dbContext.Cocktails.Count() == 0)
            {
                foreach (var item in listOfDrinks)
                {
                    var photo = File.ReadAllBytes(item.PhotoPath);
                    CreateDrink(item.Name, item.Description, item.Ingredients, photo);
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
            await dbContext.Cocktails.AddAsync(cocktail);
            await dbContext.SaveChangesAsync();

            var ingCounter = 0;
            foreach (var item in ingredients)
            {
                if (await dbContext.Ingredients.Where(p => p.Name == item.ToLower()).CountAsync() == 0)
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
            var cocktail = await dbContext.Cocktails.FirstAsync(p=>p.Name==cocktailName);
            var ingredient = await dbContext.Ingredients.FirstAsync(p => p.Name == ingredientName);
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

            await dbContext.Ingredients.AddAsync(ingredient);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Ingredient> GetIngredientAsync(string name)
        {
            var ingredient = await dbContext.Ingredients.Where(p => p.Name == name.ToLower()).FirstAsync();
            return ingredient;
        }





        // Non-Async version of methods for Pre-Load

        public void CreateDrink(string name, string description, string[] ingredients, byte[] photo)
        {
            var cocktail = new Cocktail()
            {
                Name = name,
                Description = description,
                Photo = photo
            };
            dbContext.Cocktails.Add(cocktail);
            dbContext.SaveChanges();

            var ingCounter = 0;
            foreach (var item in ingredients)
            {
                if (dbContext.Ingredients.Where(p => p.Name == item.ToLower()).Count() == 0)
                {
                    if (ingCounter == 0)
                        CreateIngredient(item, 1);
                    else
                        CreateIngredient(item, 0);
                }
                ingCounter++;
                AddIngredientToCocktail(cocktail.Name, item.ToLower());
            }
        }

        public void AddIngredientToCocktail(string cocktailName, string ingredientName)
        {
            var cocktail = dbContext.Cocktails.First(p => p.Name == cocktailName);
            var ingredient = dbContext.Ingredients.First(p => p.Name == ingredientName);
            var link = new CocktailIngredient()
            {
                Cocktail = cocktail,
                Ingredient = ingredient
            };
            dbContext.CocktailIngredient.Add(link);
            dbContext.SaveChanges();
        }

        public void CreateIngredient(string name, byte primary)
        {
            var ingredient = new Ingredient()
            {
                Name = name.ToLower(),
                Primary = primary
            };

            dbContext.Ingredients.Add(ingredient);
            dbContext.SaveChanges();
        }

        public Ingredient GetIngredient(string name)
        {
            var ingredient = dbContext.Ingredients.Where(p => p.Name == name.ToLower()).First();
            return ingredient;
        }

    }
}
