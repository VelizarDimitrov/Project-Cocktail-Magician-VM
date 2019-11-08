using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
   public interface ICityService
    {
        Task CreateCityAsync(string cityName, string countryName);
        void CreateCity(string cityName, string countryName);
        Task<IList<string>> GetAllCityNames();
        Task<bool> CheckifCityNameIsCorrect(string cityName);
    }
}
