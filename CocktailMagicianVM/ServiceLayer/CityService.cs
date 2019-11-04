using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CityService:ICityService
    {
        private readonly CocktailDatabaseContext dbContext;
        private readonly ICountryService countryService;

        public CityService(CocktailDatabaseContext dbContext,ICountryService countryService)
        {
            this.dbContext = dbContext;
            this.countryService = countryService;
        }

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
    }
}
