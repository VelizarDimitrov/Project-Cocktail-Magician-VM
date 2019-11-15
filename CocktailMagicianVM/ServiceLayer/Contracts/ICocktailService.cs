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
        Task CreateCocktailAsync(string name, string description, string[] primaryIngredients, string[] ingredients, byte[] photo);
        Task AddIngredientToCocktailAsync(string cocktailName, string ingredientName, byte ingredientPrimary);
        Task CreateIngredientAsync(string name, byte primary);
        Task<Ingredient> GetIngredientAsync(string name);
        Task<IList<Ingredient>> GetAllMainIngredients();
        Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient);
        Task<IList<string>> GetAllIngredientNamesAsync();
        Task<byte[]> FindCocktailPhotoAsync(int id);
        //Task AddCocktailAsync(string name, string[] primaryIngredients, string[] ingredients, string description, byte[] cocktailPhoto);
    }
}
