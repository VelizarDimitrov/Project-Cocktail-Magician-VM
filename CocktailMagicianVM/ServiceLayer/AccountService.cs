﻿using Data;
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
        private readonly IBarService barService;
        private readonly CocktailDatabaseContext dbContext;
        public AccountService(CocktailDatabaseContext dbContext, IHashing hasher, ICountryService countryService, ICityService cityService, IBarService barService)
        {
            this.hasher = hasher;
            this.countryService = countryService;
            this.cityService = cityService;
            this.barService = barService;
            this.dbContext = dbContext;
        }

        //public async  Task<IList<User>> FindAllUsersAsync()
        //{
        //  List<User> users = await dbContext.Users.ToListAsync();
        //    return users;
        //}
        public async Task AddAccountAsync(string userName, string firstName, string lastName, string password, string accountType, string countryName, string cityName)
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
            var city = await dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).FirstAsync();
            var user = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = hasher.Hash(password),
                AccountType = accountType,
                AccountStatus = "Active",
                Country = country,
                City = city
            };
            await dbContext.Users.AddAsync(user);
            var userPhoto = new UserPhoto()
            {
                User = user
            };
            await dbContext.UserPhotos.AddAsync(userPhoto);
            await dbContext.SaveChangesAsync();

        }

        public async Task<User> FindUserByUserName(string userName)
        {
            var user = await dbContext.Users
                 .Where(p => p.UserName == userName)
                 .FirstOrDefaultAsync();
            return user;
        }

        public void DatabaseUserFill()
        {
            string userstoread = File.ReadAllText(@"../../../../Data/SolutionPreload/Users.json");
            List<UserJson> listOfUsers = JsonConvert.DeserializeObject<List<UserJson>>(userstoread);
            if (dbContext.Users.Count() == 0)
            {
                foreach (var item in listOfUsers)
                {
                    AddAccount(item.UserName, item.FirstName, item.LastName, item.Password, item.AccountType, item.Country, item.City);
                }

            }

        }
        // Non-Async version of methods for Pre-Load
        public void AddAccount(string userName, string firstName, string lastName, string password, string accountType, string countryName, string cityName)
        {
            if (dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).Count() == 0)
            {
                countryService.CreateCountry(countryName);
            }

            if (dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).Count() == 0)
            {
                cityService.CreateCity(cityName, countryName);
            }

            var country = dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).First();
            var city = dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).First();
            var user = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = hasher.Hash(password),
                AccountType = accountType,
                AccountStatus = "Active",
                Country = country,
                City = city
            };
            dbContext.Users.Add(user);
            var userPhoto = new UserPhoto()
            {
                User = user
            };
            dbContext.UserPhotos.Add(userPhoto);
            dbContext.SaveChanges();

        }
        public async Task<User> FindUserWebAsync(string userName, string password)
        {
            var user = await dbContext.Users
               .Where(p => p.UserName == userName)
               .FirstOrDefaultAsync();
            if (user == null || !this.hasher.Verify(password, user.Password))
            {
                throw new ArgumentException("username or password incorrect");
            }

            return user;
        }

        public async Task RateBarAsync(int userId, int userRating, int barId)
        {
            var user = await FindUserByIdAsync(userId);
            var bar = await barService.FindBarByIdAsync(barId);
            var barRating = new BarRating()
            {
                User = user,
                Bar = bar,
                Rating = userRating

            };
            await dbContext.BarRating.AddAsync(barRating);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> FindUserByIdAsync(int userId) =>
            await dbContext.Users.Where(p => p.Id == userId).FirstAsync();
    }
}
