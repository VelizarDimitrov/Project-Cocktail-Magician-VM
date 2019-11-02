using Data;
using Data.SolutionPreLoad.JsonParsers;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;

namespace ServiceLayer
{
    public class BarService:IBarService
    {
        private readonly CocktailDatabaseContext dbContext;
        private readonly ICountryService countryService;
        private readonly ICityService cityService;

        public BarService(CocktailDatabaseContext dbContext,ICountryService countryService,ICityService cityService)
        {
            this.dbContext = dbContext;
            this.countryService = countryService;
            this.cityService = cityService;
        }
        public async Task DatabaseBarFillASync()
        {
            string barstoread = File.ReadAllText(@"../../../../Data/SolutionPreload/Bars.json");
            List<BarJson> listOfBars = JsonConvert.DeserializeObject<List<BarJson>>(barstoread);

            if ((await BarsCountAsync()) == 0)
            {
                foreach (var item in listOfBars)
                {
                    byte[] barCover =File.ReadAllBytes(item.BarCoverPath);
                    await AddBarAsync(item.Name, item.Address, item.Description, item.Country, item.City,barCover);
                   
                }

            }


        }

        public async Task AddBarAsync(string name, string address, string description, string countryName, string cityName, byte[] barCover)
        {
            if (await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                await countryService.CreateCountryAsync(countryName);
            }

            if (await dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).CountAsync() == 0)
            {
                await cityService.CreateCityAsync(cityName, countryName);
            }

            var country = await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();
            var city = await dbContext.Cities.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();
            var bar = new Bar()
            {
                Name = name,
                Address=address,
                Description=description,
                Country = country,
                City = city
            };
            await dbContext.Bars.AddAsync(bar);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> BarsCountAsync()
        {
            var bars = await dbContext.Bars.CountAsync();
            return bars;
        }
    }
}
