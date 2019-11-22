using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CityService : ICityService
    {
        private readonly CocktailDatabaseContext dbContext;
        private readonly ICountryService countryService;

        public CityService(CocktailDatabaseContext dbContext, ICountryService countryService)
        {
            this.dbContext = dbContext;
            this.countryService = countryService;
        }

        // Non-Async version of methods for Pre-Load
        public void CreateCity(string cityName, string countryName)
        {
            if (dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).Count() == 0)
            {
                countryService.CreateCountry(countryName);
            }
            var country = dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstOrDefault();

            var city1 = new City()
            {
                Name = cityName,
                Country = country
            };
            dbContext.Cities.Add(city1);
            dbContext.SaveChanges();
        }

        //End of Pre-Load

        public async Task CreateCityAsync(string cityName, string countryName)
        {
            if (String.IsNullOrWhiteSpace(cityName))
                throw new ArgumentException("City name cannot be null or empty.");
            if (!(await countryService.CheckIfCountryExistsAsync(countryName)))
                await countryService.CreateCountryAsync(countryName);

            var country = await countryService.GetCountryByNameAsync(countryName);

            var newCity = new City()
            {
                Name = cityName,
                Country = country
            };
            await dbContext.Cities.AddAsync(newCity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IList<string>> GetAllCityNamesAsync() =>
            await dbContext.Cities.Select(p => p.Name).ToListAsync();

        public async Task<bool> CheckIfCityExistsAsync(string cityName) =>
            await dbContext.Cities
                            .Where(p => p.Name.ToLower() == (cityName == null ? "" : cityName.ToLower())).AnyAsync();

        public async Task<IList<string>> GetCitiesFromCountryAsync(string countryName) =>
            await dbContext.Cities
                           .Where(p => p.Country.Name.ToLower() == countryName.ToLower())
                           .Select(p => p.Name).ToListAsync();

        public async Task<City> GetCityByNameAsync(string cityName) =>
            await dbContext.Cities.FirstOrDefaultAsync(p => p.Name.ToLower() == cityName.ToLower());
    }
}
