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
        private readonly IBarService barService;
        private readonly ICocktailService cService;
        private readonly CocktailDatabaseContext dbContext;
        public AccountService(CocktailDatabaseContext dbContext, IHashing hasher, ICountryService countryService, ICityService cityService, IBarService barService, ICocktailService cService)
        {
            this.hasher = hasher;
            this.countryService = countryService;
            this.cityService = cityService;
            this.barService = barService;
            this.cService = cService;
            this.dbContext = dbContext;
        }

        //public async  Task<IList<User>> FindAllUsersAsync()
        //{
        //  List<User> users = await dbContext.Users.ToListAsync();
        //    return users;
        //}
        public async Task AddAccountAsync(string userName, string firstName, string lastName, string password, string accountType, string countryName, string cityName)
        {

            var user = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = hasher.Hash(password),
                AccountType = accountType,
                AccountStatus = "Active",
                Country = countryName,
                City = cityName,
                Frozen = 0
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

            var user = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Password = hasher.Hash(password),
                AccountType = accountType,
                AccountStatus = "Active",
                Country = countryName,
                City = cityName,
                Frozen = 0
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
           await SetLastLoginAsync(user.Id);
            return user;
        }

        public async Task RateBarAsync(int userId, int userRating, int barId)
        {
            if (await dbContext.BarRating.AnyAsync(p => (p.UserId == userId && p.BarId == barId)))
            {
                var givenRating = await dbContext.BarRating.FirstOrDefaultAsync(p => (p.UserId == userId && p.BarId == barId));
                givenRating.Rating = userRating;
                await dbContext.SaveChangesAsync();
                await barService.UpdateAverageRatingAsync(barId);
            }
            else
            {
                var user = await FindUserByIdAsync(userId);
                var bar = await barService.FindBarByIdAsync(barId);
                var barRating = new BarRating()
                {
                    User = user,
                    Bar = bar,
                    Rating = userRating,
                    BarName = bar.Name,
                    UserName = user.UserName

                };
                await dbContext.BarRating.AddAsync(barRating);
                await dbContext.SaveChangesAsync();
                await barService.UpdateAverageRatingAsync(barId);
            }
        }

        public async Task<User> FindUserByIdAsync(int userId) =>
            await dbContext.Users.Where(p => p.Id == userId)
            .Include(p => p.UserPhoto)
            .Include(p => p.Notifications)
            .Include(p => p.FavoriteCocktails)
            .Include(p => p.FavoriteBars)
            .Include(p => p.CocktailRatings)
            .Include(p => p.CocktailComments)
            .Include(p => p.BarRatings)
            .Include(p => p.BarComments)
            .FirstOrDefaultAsync();

        public async Task AddBarCommentAsync(int id, string createComment, int userId)
        {
            if (await dbContext.BarComment.AnyAsync(p => (p.UserId == userId && p.BarId == id)))
            {
                var givenComment = await dbContext.BarComment.FirstOrDefaultAsync(p => (p.UserId == userId && p.BarId == id));
                givenComment.Comment = createComment;
                givenComment.CreatedOn = DateTime.Now;
                await dbContext.SaveChangesAsync();

            }
            else
            {
                var user = await FindUserByIdAsync(userId);
                var bar = await barService.FindBarByIdAsync(id);
                var barComment = new BarComment()
                {
                    User = user,
                    Bar = bar,
                    UserUserName = user.UserName,
                    BarName = bar.Name,
                    Comment = createComment,
                    CreatedOn = DateTime.Now

                };
                await dbContext.BarComment.AddAsync(barComment);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SetLastLoginAsync(int id)
        {
            var user = await FindUserByIdAsync(id);
            user.LastLogIn = DateTime.Now;
            await dbContext.SaveChangesAsync();
        }

        public async Task AddCocktailCommentAsync(int id, string createComment, int userId)
        {
            if (await dbContext.CocktailComment.AnyAsync(p => (p.UserId == userId && p.CocktailId == id)))
            {
                var existingComment = await dbContext.CocktailComment.FirstOrDefaultAsync(p => (p.UserId == userId && p.CocktailId == id));
                existingComment.Comment = createComment;
                existingComment.CreatedOn = DateTime.Now;
                await dbContext.SaveChangesAsync();

            }
            else
            {
                var user = await FindUserByIdAsync(userId);
                var cocktail = await cService.FindCocktailByIdAsync(id);
                var cocktailComment = new CocktailComment()
                {
                    User = user,
                    Cocktail = cocktail,
                    UserName = user.UserName,
                    CocktailName = cocktail.Name,
                    Comment = createComment,
                    CreatedOn = DateTime.Now

                };
                await dbContext.CocktailComment.AddAsync(cocktailComment);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RateCocktailAsync(int userId, int rating, int cocktailId)
        {
            if (await dbContext.CocktailRating.AnyAsync(p => (p.UserId == userId && p.CocktailId == cocktailId)))
            {
                var givenRating = await dbContext.CocktailRating.FirstOrDefaultAsync(p => (p.UserId == userId && p.CocktailId == cocktailId));
                givenRating.Rating = rating;
                await dbContext.SaveChangesAsync();
                await cService.UpdateAverageRatingAsync(cocktailId);
            }
            else
            {
                var user = await FindUserByIdAsync(userId);
                var cocktail = await cService.FindCocktailByIdAsync(cocktailId);
                var cocktailRating = new CocktailRating()
                {
                    User = user,
                    Cocktail = cocktail,
                    Rating = rating,
                    CocktailName = cocktail.Name,
                    UserUserName = user.UserName

                };
                await dbContext.CocktailRating.AddAsync(cocktailRating);
                await dbContext.SaveChangesAsync();
                await cService.UpdateAverageRatingAsync(cocktailId);
            }
        }

        public async Task<byte[]> FindUserAvatar(int userId) =>
            (await dbContext.UserPhotos.FirstOrDefaultAsync(p => p.UserId == userId)).UserCover;

        public async Task UpdateProfileAsync(int userId, string userName, string firstName, string lastName, byte[] userPhoto)
        {
            var user = await FindUserByIdAsync(userId);
            user.UserName = userName;
            user.FirstName = firstName;
            user.LastName = lastName;
            if (userPhoto != null)
            {
                user.UserPhoto.UserCover = userPhoto;
            }
            await dbContext.SaveChangesAsync();
        }
        public async Task<bool> VerifyUserPasswordAsync(string userName, string password)
        {
            var user = await dbContext.Users
               .Where(p => p.UserName == userName)
               .FirstOrDefaultAsync();
            bool correct = true;
            if (!this.hasher.Verify(password, user.Password))
            {
                correct = false;
            }
           
            return correct;
        }
       public async Task<bool> ValidateUserPasswordAsync(int userId, string password)
        {
            var user = await FindUserByIdAsync(userId);
            var validate = await VerifyUserPasswordAsync(user.UserName, password);
            return validate;
        }
        public async Task UpdatePasswordAsync(int userId, string password)
        {
            var user = await FindUserByIdAsync(userId);
            user.Password = hasher.Hash(password);
            await dbContext.SaveChangesAsync();
        }
    }
}
