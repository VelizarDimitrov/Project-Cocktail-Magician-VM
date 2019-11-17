using Data.Contracts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
   public interface ICountryService
    {
        Task CreateCountryAsync(string countryName);
        void CreateCountry(string countryName);
        Task<IList<string>> GetAllCountryNamesAsync();
        Task<bool> CheckIfCountryExistsAsync(string countryName);
        Task<Country> GetCountryByNameAsync(string countryName);
    }
}
