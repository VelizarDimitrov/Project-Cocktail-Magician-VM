﻿using Data;
using Data.Contracts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CountryService : ICountryService
    {
        private readonly CocktailDatabaseContext dbContext;

        public CountryService(CocktailDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // Non-Async version of methods for Pre-Load
        public void CreateCountry(string countryName)
        {
            if (!dbContext.Countries.Any(p => p.Name.ToLower() == countryName.ToLower()))
            //if (dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).Count() == 0)
            {
                var country1 = new Country()
                {
                    Name = countryName
                };
                dbContext.Countries.Add(country1);
                dbContext.SaveChanges();
            }
            else
                throw new ArgumentException("Country with that name already exists!");
        }
        //End of Pre-Load

        public async Task CreateCountryAsync(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
            {
                throw new ArgumentNullException("Country name cannot be null or whitespace.");
            }
            if (!(await CheckIfCountryExistsAsync(countryName)))
            {
                var country1 = new Country()
                {
                    Name = countryName
                };
                await dbContext.Countries.AddAsync(country1);
                await dbContext.SaveChangesAsync();
            }
            else
                throw new ArgumentException("Country with that name already exists!");
        }
        public async Task<IList<string>> GetAllCountryNamesAsync() =>
             await dbContext.Countries.Select(p => p.Name).ToListAsync();

        public async Task<bool> CheckIfCountryExistsAsync(string countryName) =>
            await dbContext.Countries.AnyAsync(p => p.Name == countryName);

        public async Task<Country> GetCountryByNameAsync(string countryName) =>
            await dbContext.Countries.FirstOrDefaultAsync(p => p.Name.ToLower() == countryName.ToLower());
    }
}