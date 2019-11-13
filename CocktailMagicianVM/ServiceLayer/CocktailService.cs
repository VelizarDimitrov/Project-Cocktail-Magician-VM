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


        // Non-Async version of methods for Pre-Load

        public void DatabaseCocktailFill()
        {
            string drinksToRead = File.ReadAllText(@"../../../../Data/SolutionPreload/Cocktails.json");
            List<CocktailJson> listOfDrinks = JsonConvert.DeserializeObject<List<CocktailJson>>(drinksToRead);

            foreach (var item in listOfDrinks)
            {
                item.PhotoPath = @"../../../../Data/SolutionPreload/CocktailPhotos/" + String.Join('-', item.Name.Split(" ")).ToLower() + ".jpg";
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

        public void CreateDrink(string name, string description, string[] ingredients, byte[] photo)
        {
            var cocktail = new Cocktail()
            {
                Name = name,
                Description = description,
                AverageRating = 0
            };
            dbContext.Cocktails.Add(cocktail);
            var cocktailPhoto = new CocktailPhoto()
            {
                CocktailCover = photo,
                Cocktail = cocktail
            };
            dbContext.CocktailPhotos.Add(cocktailPhoto);
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
                Ingredient = ingredient,
                CocktailName = cocktail.Name,
                IngredientName = ingredient.Name
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

        //End of Pre-Load

        public async Task CreateDrinkAsync(string name, string description, string[] ingredients, byte[] photo)
        {
            var cocktail = new Cocktail()
            {
                Name = name,
                Description = description,
                AverageRating = 0
            };
            await dbContext.Cocktails.AddAsync(cocktail);
            var cocktailPhoto = new CocktailPhoto()
            {
                CocktailCover = photo,
                Cocktail = cocktail
            };
            await dbContext.CocktailPhotos.AddAsync(cocktailPhoto);
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
            var cocktail = await dbContext.Cocktails.FirstAsync(p => p.Name == cocktailName);
            var ingredient = await dbContext.Ingredients.FirstAsync(p => p.Name == ingredientName);
            var link = new CocktailIngredient()
            {
                Cocktail = cocktail,
                Ingredient = ingredient,
                CocktailName = cocktail.Name,
                IngredientName = ingredient.Name
            };
            await dbContext.CocktailIngredient.AddAsync(link);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateIngredientAsync(string name, byte primary)
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

        public async Task<IList<Ingredient>> GetAllMainIngredients()
        {
            var ingredients = await dbContext.Ingredients.Where(p => p.Primary == 1).ToListAsync();
            return ingredients;
        }

        public async Task<Tuple<IList<Cocktail>, bool>> FindCocktailByNameAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient)
        {
            bool lastPage = true;
            List<Cocktail> cocktails;
            var ratings = rating.Split(';');
            var a = int.Parse(ratings[0]);
            var b = int.Parse(ratings[1]);
            cocktails = await dbContext.Cocktails
                .Include(p => p.Ingredients)
                .Include(p => p.Bars)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.FavoritedBy)
               .Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
               .Where(p => p.Ingredients.Where(x => x.IngredientName.Contains(mainIngredient)).Any())
               .Where(p => p.AverageRating >= a)
               .Where(p => p.AverageRating <= b)
               .ToListAsync();
            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Name).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Ingredient":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Ingredients.First().IngredientName).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Ingredients.First()).ToList();
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.AverageRating).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.AverageRating).ToList();
                    break;
                default:
                    break;
            }
            cocktails = cocktails.Skip((page - 1) * 12).ToList();
            if (cocktails.Count > 12)
            {
                lastPage = false;
            }
            cocktails = cocktails.Take(12).ToList();
            return new Tuple<IList<Cocktail>, bool>(cocktails, lastPage);
        }

        public async Task<Tuple<IList<Cocktail>, bool>> FindCocktailByBarAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient)
        {
            bool lastPage = true;
            List<Cocktail> cocktails;
            var ratings = rating.Split(';');
            var a = int.Parse(ratings[0]);
            var b = int.Parse(ratings[1]);
            cocktails = await dbContext.Cocktails
                .Include(p => p.Ingredients)
                .Include(p => p.Bars)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.FavoritedBy)
               .Where(p => p.Bars.Where(x => x.BarName.Contains(keyword.ToLower())).Any())
               .Where(p => p.Ingredients.Where(x => x.IngredientName.Contains(mainIngredient)).Any())
               .Where(p => p.AverageRating >= a)
               .Where(p => p.AverageRating <= b)
               .ToListAsync();
            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Name).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Ingredient":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Ingredients.First().IngredientName).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Ingredients.First()).ToList();
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.AverageRating).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.AverageRating).ToList();
                    break;
                default:
                    break;
            }
            cocktails = cocktails.Skip((page - 1) * 12).ToList();
            if (cocktails.Count > 12)
            {
                lastPage = false;
            }
            cocktails = cocktails.Take(12).ToList();
            return new Tuple<IList<Cocktail>, bool>(cocktails, lastPage);
        }

        public async Task<Tuple<IList<Cocktail>, bool>> FindCocktailByIngredientAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient)
        {
            bool lastPage = true;
            List<Cocktail> cocktails;
            var ratings = rating.Split(';');
            var a = int.Parse(ratings[0]);
            var b = int.Parse(ratings[1]);
            cocktails = await dbContext.Cocktails
                .Include(p => p.Ingredients)
                .Include(p => p.Bars)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.FavoritedBy)
               .Where(p => p.Ingredients.Where(x => x.IngredientName.Contains(keyword.ToLower())).Any())
               .Where(p => p.Ingredients.Where(x => x.IngredientName.Contains(mainIngredient)).Any())
               .Where(p => p.AverageRating >= a)
               .Where(p => p.AverageRating <= b)
               .ToListAsync();
            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Name).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Name).ToList();
                    break;
                case "Ingredient":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Ingredients.First().IngredientName).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Ingredients.First()).ToList();
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.AverageRating).ToList();
                    else
                        cocktails = cocktails.OrderByDescending(p => p.AverageRating).ToList();
                    break;
                default:
                    break;
            }
            cocktails = cocktails.Skip((page - 1) * 12).ToList();
            if (cocktails.Count > 12)
            {
                lastPage = false;
            }
            cocktails = cocktails.Take(12).ToList();
            return new Tuple<IList<Cocktail>, bool>(cocktails, lastPage);
        }

        public async Task<byte[]> FindCocktailPhotoAsync(int id)
        {
            var picture = await dbContext.CocktailPhotos.FirstAsync(p => p.CocktailId == id);
            return picture.CocktailCover;
        }
    }
}
