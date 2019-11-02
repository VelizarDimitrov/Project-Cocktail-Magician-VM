using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
   public interface ICountryService
    {
        Task CreateCountryAsync(string countryName);
    }
}
