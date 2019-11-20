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
        Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder, int pageSize);
        Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, int page, int pageSize, int userId);
        Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, int page, int pageSize);
        Task<IList<Bar>> GetNewestBarsAsync();
        Task UpdateAverageRatingAsync(int barId);
        Task<IList<BarComment>> GetBarCommentsAsync(int barId, int loadNumber);
        Task<Bar> FindBarByNameAsync(string barName);
        Task HideBarAsync(int id);
        Task UnhideBarAsync(int id);
        Task AddCocktailBarAsync(int barId, int cocktailId);
        Task RemoveCoctailBarAsync(int barId, int cocktailId);
    }
}
