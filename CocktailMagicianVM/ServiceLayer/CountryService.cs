using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CountryService:ICountryService
    {
        private readonly CocktailDatabaseContext dbContext;
        private readonly ICityService cityService;

        public CountryService(CocktailDatabaseContext dbContext, ICityService cityService)
        {
            this.dbContext = dbContext;
            this.cityService = cityService;
        }
        public async Task CreateCountryAsync(string countryName)
        {
            if (await dbContext.Country.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                var country1 = new Country()
                {
                    Name = countryName
                };
                await dbContext.Country.AddAsync(country1);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Country with that name already exists!");
            }
        }
    }
}
