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
        Task<IList<Ingredient>> GetAllMainIngredients();
        Task<Tuple<IList<Cocktail>, bool>> FindCocktailByNameAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient);
        Task<IList<string>> GetAllIngredientNamesAsync();
        Task<Tuple<IList<Cocktail>, bool>> FindCocktailByBarAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient);
        Task<Tuple<IList<Cocktail>, bool>> FindCocktailByIngredientAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient);
        Task<byte[]> FindCocktailPhotoAsync(int id);
        //Task AddCocktailAsync(string name, string[] primaryIngredients, string[] ingredients, string description, byte[] cocktailPhoto);
    }
}
