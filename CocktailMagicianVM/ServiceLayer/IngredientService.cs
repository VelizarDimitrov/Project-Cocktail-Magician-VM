using Data;
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
            dbContext.Ingredients.Where(p => p.Name.ToLower() == name.ToLower()).First();

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

        public async Task<Ingredient> GetIngredientByNameAsync(string name) =>
            await dbContext.Ingredients.Where(p => p.Name == name.ToLower()).FirstAsync();

        public async Task<IList<Ingredient>> GetAllMainIngredientsAsync() =>
            await dbContext.Ingredients.Where(p => p.Primary == 1).ToListAsync();

        public async Task<IList<string>> GetAllIngredientNamesAsync() =>
            await dbContext.Ingredients.Select(p => p.Name).ToListAsync();

        public async Task<Ingredient> GetIngredientByNameTypeAsync(string name, int primary) =>
            await dbContext.Ingredients.FirstAsync(p => (p.Name.ToLower() == name.ToLower() && p.Primary == primary));
    }
}
