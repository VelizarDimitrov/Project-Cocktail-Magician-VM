using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface ICocktailService
    {
        void DatabaseCocktailFill();
        Task CreateDrinkAsync(string name, string description, string[] ingredients, byte[] photo);
        Task AddIngredientToCocktailAsync(string cocktailName, string ingredientName);
        Task CreateIngredientAsync(string name, byte primary);
        Task<Ingredient> GetIngredientAsync(string name);
    }
}
