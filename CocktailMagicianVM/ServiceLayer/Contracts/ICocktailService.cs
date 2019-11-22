using Data.Contracts;
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
        Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder, string mainIngredient, int pageSize);
        Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForUserAsync(string keyword, int page, int pageSize, int? userId);
        //Task<Tuple<IList<Cocktail>, bool>> FindCocktailsForCatalogAsync(string keyword, int page, int pageSize);
        Task<CocktailPhoto> FindCocktailPhotoAsync(int id);
        Task<Cocktail> FindCocktailByIdAsync(int id);
        Task<IList<CocktailComment>> GetCocktailCommentsAsync(int id, int loadNumber);
        Task UpdateAverageRatingAsync(int cocktailId);
        Task HideCocktailAsync(int id);
        Task UnhideCocktailAsync(int id);
        Task UpdateCocktailAsync(int id, string name, string description, string[] primaryIngredientsArr, string[] ingredientsArr, byte[] cocktailPhoto);
        //Task AddCocktailAsync(string name, string[] primaryIngredients, string[] ingredients, string description, byte[] cocktailPhoto);
    }
}
