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
            if (await dbContext.Country.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                await countryService.CreateCountryAsync(countryName);
            }
            var country = await dbContext.Country.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();

            var city1 = new City()
            {
                Name = cityName,
                Country = country
            };
            await dbContext.City.AddAsync(city1);
            await dbContext.SaveChangesAsync();
        }
    }
}
