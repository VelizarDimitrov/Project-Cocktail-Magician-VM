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

        public CountryService(CocktailDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task CreateCountryAsync(string countryName)
        {
            if (await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                var country1 = new Country()
                {
                    Name = countryName
                };
                await dbContext.Countries.AddAsync(country1);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Country with that name already exists!");
            }
        }
    }
}
