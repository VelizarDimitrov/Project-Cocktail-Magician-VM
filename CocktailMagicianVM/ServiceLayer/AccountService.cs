using Data;
using Data.Models;
using Data.SolutionPreLoad.JsonParsers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AccountService : IAccountService
    {
        private readonly IHashing hasher;
        private readonly ICountryService countryService;
        private readonly ICityService cityService;
        private readonly CocktailDatabaseContext dbContext;
        public AccountService(CocktailDatabaseContext dbContext, IHashing hasher,ICountryService countryService,ICityService cityService)
        {
            this.hasher = hasher;
            this.countryService = countryService;
            this.cityService = cityService;
            this.dbContext = dbContext;
        }
        public async Task DatabaseUserFillAsync()
        {
            string userstoread = File.ReadAllText(@"../../../../Data/SolutionPreload/Users.json");
            List<UserJson> listOfUsers = JsonConvert.DeserializeObject<List<UserJson>>(userstoread);
            if ((await FindAllUsersAsync()).Count == 0)
            {
                foreach (var item in listOfUsers)
                {
                  await AddAccountAsync(item.UserName, item.FirstName, item.LastName, item.Password, item.AccountType,item.Country,item.City);
                }

            }

        }
        public async  Task<IList<User>> FindAllUsersAsync()
        {
          return await dbContext.Users.ToListAsync();
        }
        public async Task  AddAccountAsync(string userName, string firstName, string lastName, string password, string accountType,string countryName,string cityName)
        {
            if (await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                await countryService.CreateCountryAsync(countryName);
            }
            
            if (await dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).CountAsync() == 0)
            {
                await cityService.CreateCityAsync(cityName,countryName);
            }

            var country =await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();
            var city =await dbContext.Cities.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();
            var user = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = hasher.Hash(password),
                AccountType = accountType,
                AccountStatus = "Active",
                Country=country,
                City=city
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

        }   
    }
}
