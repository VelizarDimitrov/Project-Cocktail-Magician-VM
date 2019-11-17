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
        Task<Ingredient> GetIngredientByNameAsync(string name);
        Task<IList<Ingredient>> GetAllMainIngredientsAsync();
    }
}
