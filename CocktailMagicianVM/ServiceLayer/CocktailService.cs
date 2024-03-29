﻿using Data;
using Data.Contracts;
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
        private readonly IIngredientService iService;

        public CocktailService(CocktailDatabaseContext dbContext, IIngredientService iService)
        {
            this.dbContext = dbContext;
            this.iService = iService;
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

            if (true)
            {
                foreach (var item in listOfDrinks)
                {
                    var photo = File.ReadAllBytes(item.PhotoPath);
                    CreateCocktail(item.Name, item.Description, item.Ingredients, photo);
                }
            }
        }

        public void CreateCocktail(string name, string description, string[] ingredients, byte[] photo)
        {
            var cocktail = new Cocktail()
            {
                Name = name,
                Description = description,
                AverageRating = 0,
                Hidden = 0
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
                if (!dbContext.Ingredients.Where(p => p.Name.ToLower() == item.ToLower()).Any())
                {
                    if (ingCounter == 0)
                        iService.CreateIngredient(item, 1);
                    else
                        iService.CreateIngredient(item, 0);
                }
                ingCounter++;
                AddIngredientToCocktail(cocktail.Name, item.ToLower());
            }
        }

        public void AddIngredientToCocktail(string cocktailName, string ingredientName)
        {
            var cocktail = dbContext.Cocktails.FirstOrDefault(p => p.Name.ToLower() == cocktailName.ToLower());
            var ingredient = iService.GetIngredient(ingredientName);
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

        //End of Pre-Load

        public async Task CreateCocktailAsync(string name, string description, string[] primaryIngredients, string[] ingredients, byte[] photo)
        {
            if (primaryIngredients==null)
            {
                throw new ArgumentNullException("Primary ingredients list cannot be null.");
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Cocktail name cannot be null or whitespace.");
            }
            var cocktail = new Cocktail()
            {
                Name = name,
                Description = description,
                AverageRating = 0,
                Hidden = 0
            };
            await dbContext.Cocktails.AddAsync(cocktail);
            var cocktailPhoto = new CocktailPhoto()
            {
                CocktailCover = photo,
                Cocktail = cocktail
            };
            await dbContext.CocktailPhotos.AddAsync(cocktailPhoto);
            await dbContext.SaveChangesAsync();

            foreach (var item in primaryIngredients.Where(p => !String.IsNullOrEmpty(p)))
            {
                if (!await iService.CheckIfIngredientExistsAsync(item, 1))
                    await iService.CreateIngredientAsync(item, 1);
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower(), 1);
            }
            foreach (var item in (ingredients ?? new string[0]).Where(p => !String.IsNullOrEmpty(p)))
            {
                if (!await iService.CheckIfIngredientExistsAsync(item, 0))
                    await iService.CreateIngredientAsync(item, 0);
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower(), 0);
            }
        }

        public async Task AddIngredientToCocktailAsync(string cocktailName, string ingredientName, byte ingredientPrimary)
        {
            if (String.IsNullOrWhiteSpace(cocktailName))
            {
                throw new ArgumentNullException("Cocktail name cannot be null or whitespace.");
            }
            var cocktail = await FindCocktailByNameAsync(cocktailName);
            var ingredient = await iService.GetIngredientByNameTypeAsync(ingredientName, ingredientPrimary);
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

        //public async Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForCatalogAsync(string keyword, int page, int pageSize)
        //{
        //    bool lastPage = true;

        //    var cocktails = dbContext.Cocktails
        //        .Include(p => p.Ingredients)
        //        .Include(p => p.Bars)
        //        .AsQueryable();

        //    cocktails = cocktails.Where(p => p.Name.ToLower().Contains(keyword.ToLower())
        //    || p.Ingredients.Any(x => x.IngredientName.ToLower().Contains(keyword.ToLower())));

        //    cocktails = cocktails.Skip((page - 1) * pageSize);
        //    var foundCocktails = await cocktails.ToListAsync();

        //    if (foundCocktails.Count > pageSize)
        //    {
        //        lastPage = false;
        //    }
        //    foundCocktails = foundCocktails.Take(pageSize).ToList();
        //    return new Tuple<IList<Cocktail>, bool>(foundCocktails, lastPage);
        //}

        public async Task<Cocktail> FindCocktailByNameAsync(string name) =>
            await dbContext.Cocktails.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

        public async Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForUserAsync(string keyword, int page, int pageSize, int? userId)
        {
            bool lastPage = true;

            var cocktails = dbContext.Cocktails
                .Include(p => p.Ingredients)
                .Include(p => p.Bars)
                .AsQueryable();

            if (userId != null)
            {
                cocktails = cocktails
                .Where(p => p.FavoritedBy.Any(x => x.UserId == userId) && p.Hidden == 0);
            }

            cocktails = cocktails.Where(p => p.Name.ToLower().Contains(keyword.ToLower())
            || p.Ingredients.Any(x => x.IngredientName.ToLower().Contains(keyword.ToLower())));

            cocktails = cocktails.Skip((page - 1) * pageSize);
            var foundCocktails = await cocktails.ToListAsync();

            if (foundCocktails.Count > pageSize)
            {
                lastPage = false;
            }
            foundCocktails = foundCocktails.Take(pageSize).ToList();
            return new Tuple<IList<Cocktail>, bool>(foundCocktails, lastPage);
        }

        public async Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient, int pageSize)
        {
            bool lastPage = true;
            var ratings = rating.Split(';');
            var minRating = int.Parse(ratings[0]);
            var maxRating = int.Parse(ratings[1]);

            var cocktails = dbContext.Cocktails
                .Include(p => p.Ingredients)
                .Include(p => p.Bars)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.FavoritedBy)
                .Where(p => p.Hidden == 0)
                .AsQueryable();

            switch (keywordCriteria)
            {
                case "Name":
                    cocktails = cocktails.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
                    break;
                case "Bar":
                    cocktails = cocktails.Where(p => p.Bars.Any(x => x.BarName.ToLower().Contains(keyword.ToLower())));
                    break;
                case "Ingredient":
                    cocktails = cocktails.Where(p => p.Ingredients.Any(x => x.IngredientName.ToLower().Contains(keyword.ToLower())));
                    break;
            }

            cocktails = cocktails
               .Where(p => p.Ingredients.Where(x => x.IngredientName.Contains(mainIngredient)).Any())
               .Where(p => p.AverageRating >= minRating)
               .Where(p => p.AverageRating <= maxRating);

            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Name);
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Name);
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.AverageRating);
                    else
                        cocktails = cocktails.OrderByDescending(p => p.AverageRating);
                    break;
                default:
                    break;
            }

            cocktails = cocktails.Skip((page - 1) * pageSize);
            var foundCocktails = await cocktails.ToListAsync();

            if (foundCocktails.Count > pageSize)
            {
                lastPage = false;
            }
            foundCocktails = foundCocktails.Take(pageSize).ToList();
            return new Tuple<IList<Cocktail>, bool>(foundCocktails, lastPage);
        }

        public async Task<CocktailPhoto> FindCocktailPhotoAsync(int id) =>
            await dbContext.CocktailPhotos.FirstOrDefaultAsync(p => p.CocktailId == id);

        public async Task<Cocktail> FindCocktailByIdAsync(int id) =>
            await dbContext.Cocktails.Include(p => p.Ingredients).Include(p => p.Bars).Include(p => p.Ratings)
                .Include(p => p.Comments).Include(p => p.FavoritedBy).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IList<CocktailComment>> GetCocktailCommentsAsync(int id, int loadNumber) =>
            await dbContext.CocktailComment
                .Where(p => p.CocktailId == id)
                .OrderByDescending(c => c.CreatedOn)
                .Take(loadNumber)
                .ToListAsync();

        public async Task UpdateAverageRatingAsync(int cocktailId)
        {
            var cocktail = await FindCocktailByIdAsync(cocktailId);
            cocktail.AverageRating = Math.Round(cocktail.Ratings.Average(p => p.Rating), 1);
            await dbContext.SaveChangesAsync();
        }

        public async Task HideCocktailAsync(int id)
        {
            var cocktail = await FindCocktailByIdAsync(id);
            cocktail.Hidden = 1;
            await dbContext.SaveChangesAsync();
        }

        public async Task UnhideCocktailAsync(int id)
        {
            var cocktail = await FindCocktailByIdAsync(id);
            cocktail.Hidden = 0;
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateCocktailAsync(int id, string name, string description, string[] primaryIngredientsArr, string[] ingredientsArr, byte[] cocktailPhoto)
        {
            var cocktail = await FindCocktailByIdAsync(id);
            var photo = await FindCocktailPhotoAsync(id);

            if (cocktailPhoto != null)
                photo.CocktailCover = cocktailPhoto;
            cocktail.Name = name;
            cocktail.Description = description;

            var oldIngredients = await iService.GetCocktailIngredientsByCocktailAsync(id);

            foreach (var ingredient in oldIngredients)
            {
                dbContext.CocktailIngredient.Remove(ingredient);
            }
            await dbContext.SaveChangesAsync();



            foreach (var item in primaryIngredientsArr.Where(p => !String.IsNullOrEmpty(p)))
            {
                if (!await iService.CheckIfIngredientExistsAsync(item, 1))
                    await iService.CreateIngredientAsync(item, 1);
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower(), 1);
            }
            foreach (var item in (ingredientsArr ?? new string[0]).Where(p => !String.IsNullOrEmpty(p)))
            {
                if (!await iService.CheckIfIngredientExistsAsync(item, 0))
                    await iService.CreateIngredientAsync(item, 0);
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower(), 0);
            }

        }
    }
}
