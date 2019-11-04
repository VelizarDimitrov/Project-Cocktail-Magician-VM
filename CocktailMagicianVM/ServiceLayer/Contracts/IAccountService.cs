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
        //Task<IList<User>> FindAllUsersAsync();
    }
}
