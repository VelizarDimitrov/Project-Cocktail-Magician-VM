using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System.Collections;
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
            var country = dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).First();

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
            if (await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                await countryService.CreateCountryAsync(countryName);
            }
            var country = await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();

            var city1 = new City()
            {
                Name = cityName,
                Country = country
            };
            await dbContext.Cities.AddAsync(city1);
            await dbContext.SaveChangesAsync();
        }
        public async Task<IList<string>> GetAllCityNamesAsync()
        {
            var cities = await dbContext.Cities.Select(p => p.Name).ToListAsync();
            return cities;
        }
        public async Task<bool> CheckifCityNameIsCorrect(string cityName)
        {
            bool cityExists = (await dbContext.Cities.Where(p => p.Name == cityName).CountAsync()).Equals(1);
            return cityExists;
        }

        public async Task<IList<string>> GetCitiesFromCountryAsync(string countryName)
        {
            var cities = await dbContext.Cities.Where(p => p.Country.Name == countryName).Select(p=>p.Name).ToListAsync();
            return cities;
        }
    }
}
