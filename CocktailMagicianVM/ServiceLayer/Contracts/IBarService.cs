using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IBarService
    {
        Task DatabaseBarFillASync();
        Task AddBarAsync(string name, string address, string description, string countryName, string cityName, byte[] barCover);
        Task<int> BarsCountAsync();
    }
}
