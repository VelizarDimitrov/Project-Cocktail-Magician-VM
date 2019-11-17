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

            if (dbContext.Cocktails.Count() == 0)
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
                if (dbContext.Ingredients.Where(p => p.Name == item.ToLower()).Any())
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
            var cocktail = dbContext.Cocktails.First(p => p.Name == cocktailName);
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

            foreach (var item in primaryIngredients)
            {
                // CheckIfIngredientExistsAsync at inggredienntServices
                if (!await dbContext.Ingredients.Where(p => (p.Name.ToLower() == item.ToLower() && p.Primary == 1)).AnyAsync())
                    await iService.CreateIngredientAsync(item, 1);
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower(), 1);
            }
            foreach (var item in ingredients)
            {
                // CheckIfIngredientExistsAsync at inggredienntServices
                if (!await dbContext.Ingredients.Where(p => (p.Name.ToLower() == item.ToLower() && p.Primary == 0)).AnyAsync())
                    await iService.CreateIngredientAsync(item, 0);
                await AddIngredientToCocktailAsync(cocktail.Name, item.ToLower(), 0);
            }
        }

        public async Task AddIngredientToCocktailAsync(string cocktailName, string ingredientName, byte ingredientPrimary)
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
                .AsQueryable();

            switch (keywordCriteria)
            {
                case "Name":
                    cocktails = cocktails.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
                    break;
                case "Bar":
                    cocktails = cocktails.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
                    break;
                case "Ingredient":
                    cocktails = cocktails.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
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
                case "Ingredient":
                    if (sortOrder == "Ascending")
                        cocktails = cocktails.OrderBy(p => p.Ingredients.First().IngredientName);
                    else
                        cocktails = cocktails.OrderByDescending(p => p.Ingredients.First().IngredientName);
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

        public async Task<byte[]> FindCocktailPhotoAsync(int id)=>
            (await dbContext.CocktailPhotos.FirstAsync(p => p.CocktailId == id)).CocktailCover;

        public async Task<Cocktail> FindCocktailByIdAsync(int id) =>
            await dbContext.Cocktails.Include(p => p.Ingredients).Include(p => p.Bars).Include(p => p.Ratings)
                .Include(p => p.Comments).Include(p => p.FavoritedBy).FirstAsync(p => p.Id == id);

        public async Task<IList<CocktailComment>> GetCocktailCommentsAsync(int id, int loadNumber)
        {
            var comments = await dbContext.CocktailComment.Where(p => p.CocktailId == id).ToListAsync();
            comments.Reverse();
            return comments.Take(loadNumber).ToList();
        }

        public async Task UpdateAverageRatingAsync(int cocktailId)
        {
            var cocktail = await FindCocktailByIdAsync(cocktailId);
            cocktail.AverageRating = Math.Round(cocktail.Ratings.Average(p => p.Rating), 1);
            await dbContext.SaveChangesAsync();
        }
    }
}
