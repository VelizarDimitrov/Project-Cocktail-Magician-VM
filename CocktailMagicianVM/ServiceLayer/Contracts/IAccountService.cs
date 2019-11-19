using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IAccountService
    {
        void DatabaseUserFill();
        Task AddAccountAsync(string userName, string firstName, string lastName, string password, string accountType, string countryName, string cityName);
        Task<User> FindUserWebAsync(string userName, string password);
        Task<User> FindUserByUserName(string username);
        Task RateBarAsync(int userId, int userRating, int barId);
        Task<User> FindUserByIdAsync(int userId);
        Task AddBarCommentAsync(int id, string createComment, int userId);
        Task AddCocktailCommentAsync(int id, string createComment, int userId);
        Task RateCocktailAsync(int userId, int rating, int cocktailId);
        Task<byte[]> FindUserAvatar(int userId);
        Task UpdateProfileAsync(int userId, string userName, string firstName, string lastName, byte[] userPhoto);
        Task SetLastLoginAsync(int id);
        //Task<IList<User>> FindAllUsersAsync();
    }
}
