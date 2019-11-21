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
       void AddAccount(string userName, string firstName, string lastName, string password, string accountType, string countryName, string cityName);
        Task<User> FindUserWebAsync(string userName, string password);
        Task<User> FindUserByUserNameAsync(string username);
        Task RateBarAsync(int userId, int userRating, int barId);
        Task<User> FindUserByIdAsync(int userId);
        Task AddBarCommentAsync(int id, string createComment, int userId);
        Task AddCocktailCommentAsync(int id, string createComment, int userId);
        Task RateCocktailAsync(int userId, int rating, int cocktailId);
        Task<byte[]> FindUserAvatar(int userId);
        Task UpdateProfileAsync(int userId, string userName, string firstName, string lastName, byte[] userPhoto);
        Task SetLastLoginAsync(int id);
        //Task<IList<User>> FindAllUsersAsync();
        Task<bool> VerifyUserPasswordAsync(string userName, string password);
        Task<Tuple<IList<User>, bool>> FindUsersForAdminAsync(string keyword, int page, int pageSize);
        Task<bool> ValidateUserPasswordAsync(int userId, string password);
        Task UpdatePasswordAsync(int userId, string password);
        Task RemoveBarFromFavoritesAsync(int barId, int userId);
        Task RemoveCocktailFromFavoritesAsync(int cocktailId, int userId);
        Task FavoriteBarAsync(int userId, int barId);
        Task FavoriteCocktailAsync(int userId, int cocktailId);
        Task UnFreezeUserAsync(int userId);
        Task<string> CheckForFavoriteBarAsync(int userId, int barId);
        Task<string> CheckForFavoriteCocktailAsync(int userId, int cocktailId);
        Task FreezeUserAsync(int userId);
        Task PromoteUserAsync(int userId);
        Task DemoteUserAsync(int userId);
    }
}
