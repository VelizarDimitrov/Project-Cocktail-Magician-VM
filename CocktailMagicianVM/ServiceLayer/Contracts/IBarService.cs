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

        Task<IList<string>> GetAllBarNamesAsync();
        Task<IList<string>> GetBarsFromCityAsync(string cityName);
        Task<byte[]> FindBarPhotoAsync(int id);
        Task<Bar> FindBarByIdAsync(int id);
        Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder);

        Task<IList<Bar>> GetNewestBarsAsync();
    }
}
