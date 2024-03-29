﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IIngredientService
    {
        void CreateIngredient(string name, byte primary);
        Ingredient GetIngredient(string name);
        Task<IList<string>> GetAllIngredientNamesAsync();
        Task CreateIngredientAsync(string name, byte primary);
        //Task<Ingredient> GetIngredientByNameAsync(string name);
        Task<Ingredient> GetIngredientByNameTypeAsync(string name, int primary);
        Task<IList<Ingredient>> GetAllMainIngredientsAsync();
        Task<IList<Ingredient>> GetIngredientsByCocktailAsync(int CocktailId);
        Task<bool> CheckIfIngredientExistsAsync(string name, byte primary);
        Task<IList<CocktailIngredient>> GetCocktailIngredientsByCocktailAsync(int CocktailId);
        Task<Tuple<IList<Ingredient>, bool>> FindIngredientsForCatalogAsync(string keyword, int page, int pageSize);
        Task<Ingredient> FindIngredientByIdAsync(int id);
        Task<List<Ingredient>> FindIngredientsByNameAsync(string name);
        Task RemoveIngredientAsync(int ingredientId);
    }
}
