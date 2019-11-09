using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IBarService
    {
        void DatabaseBarFill();
        Task AddBarAsync(string name, string address, string description, string countryName, string cityName, byte[] barCover);
        Task<int> BarsCountAsync();

        Task<IList<string>> GetAllBarNames();
        Task<IList<string>> GetBarsFromCity(string cityName);
        Task<byte[]> FindBarPhoto(int id);
        Task<Bar> FindBarByIdAsync(int id);
        Task<Tuple<IList<Bar>, bool>> FindBarByNameAsync(string keyword, int page, string selectedOrderBy, string rating);
        Task<Tuple<IList<Bar>, bool>> FindBarByAddressAsync(string keyword, int page, string selectedOrderBy, string rating);
        Task<Tuple<IList<Bar>, bool>> FindBarByCityAsync(string keyword, int page, string selectedOrderBy, string rating);
        
    }
}
