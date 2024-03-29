﻿using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class IngredientService : IIngredientService
    {
        private readonly CocktailDatabaseContext dbContext;

        public IngredientService(CocktailDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Methods for Pre-Load
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

        public Ingredient GetIngredient(string name) =>
            dbContext.Ingredients.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();

        // End of Pre-Load

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

        //public async Task<Ingredient> GetIngredientByNameAsync(string name) =>
        //    await dbContext.Ingredients.Where(p => p.Name == name.ToLower()).FirstOrDefaultAsync();

        public async Task<IList<Ingredient>> GetAllMainIngredientsAsync() =>
            await dbContext.Ingredients.Where(p => p.Primary == 1).ToListAsync();

        public async Task<IList<string>> GetAllIngredientNamesAsync() =>
            await dbContext.Ingredients.Select(p => p.Name).ToListAsync();

        public async Task<Ingredient> GetIngredientByNameTypeAsync(string name, int primary) =>
            await dbContext.Ingredients.FirstOrDefaultAsync(p => (p.Name.ToLower() == name.ToLower() && p.Primary == primary));

        public async Task<IList<Ingredient>> GetIngredientsByCocktailAsync(int CocktailId) =>
            await dbContext.Ingredients.Include(p => p.Cocktails).Where(p => p.Cocktails.Any(x => x.CocktailId == CocktailId)).ToListAsync();

        public async Task<bool> CheckIfIngredientExistsAsync(string name, byte primary) =>
            await dbContext.Ingredients.Where(p => (p.Name.ToLower() == name.ToLower() && p.Primary == primary)).AnyAsync();

        public async Task<IList<CocktailIngredient>> GetCocktailIngredientsByCocktailAsync(int CocktailId) =>
            await dbContext.CocktailIngredient.Where(x => x.CocktailId == CocktailId).ToListAsync();
       public async Task<Tuple<IList<Ingredient>, bool>> FindIngredientsForCatalogAsync(string keyword, int page, int pageSize)
        {
            bool lastPage = true;

            var ingredients = dbContext.Ingredients
                .Include(p=>p.Cocktails)
                .AsQueryable();

            ingredients = ingredients.Where(p =>
            p.Name.ToLower().Contains(keyword.ToLower()));

            ingredients = ingredients.Skip((page - 1) * pageSize);
            var foundIngredients = await ingredients.ToListAsync();

            if (foundIngredients.Count > pageSize)
            {
                lastPage = false;
            }
            foundIngredients = foundIngredients.Take(pageSize).ToList();
            return new Tuple<IList<Ingredient>, bool>(foundIngredients, lastPage);
        }
       public async Task<Ingredient> FindIngredientByIdAsync(int id)
        {
            var ingredient = await dbContext.Ingredients.Where(p => p.Id == id).FirstOrDefaultAsync();
            return ingredient;
        }
       public async Task<List<Ingredient>> FindIngredientsByNameAsync(string name)
        {         
         var ingredients = await dbContext.Ingredients.Where(p => p.Name.ToLower() == name.ToLower()).ToListAsync();
            return ingredients;          
        }
      public async Task RemoveIngredientAsync(int ingredientId)
        {
            var ingredient = await dbContext.Ingredients.Where(p => p.Id == ingredientId).FirstOrDefaultAsync();
            dbContext.Ingredients.Remove(ingredient);
            await dbContext.SaveChangesAsync();
        }
    }
}
