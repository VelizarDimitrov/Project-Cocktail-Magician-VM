using Data;
using Data.SolutionPreLoad.JsonParsers;
using Newtonsoft.Json;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;

namespace ServiceLayer
{
    public class BarService : IBarService
    {
        private readonly CocktailDatabaseContext dbContext;
        private readonly ICountryService countryService;
        private readonly ICityService cityService;

        public BarService(CocktailDatabaseContext dbContext, ICountryService countryService, ICityService cityService)
        {
            this.dbContext = dbContext;
            this.countryService = countryService;
            this.cityService = cityService;
        }

        // Non-Async version of methods for Pre-Load

        public void DatabaseBarFill()
        {
            string barstoread = File.ReadAllText(@"../../../../Data/SolutionPreload/Bars.json");
            List<BarJson> listOfBars = JsonConvert.DeserializeObject<List<BarJson>>(barstoread);

            if (dbContext.Bars.Count() == 0)
            {
                foreach (var item in listOfBars)
                {
                    byte[] barCover = File.ReadAllBytes(item.BarCoverPath);
                    AddBar(item.Name, item.Address, item.Description, item.Country, item.City, barCover);
                }

            }


        }

        public void AddBar(string name, string address, string description, string countryName, string cityName, byte[] barCover)
        {
            if (dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).Count() == 0)
            //if (this.countryService.CheckIfCountryNameIsCorrect(countryName))
            {
                countryService.CreateCountry(countryName);
            }

            if (dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).Count() == 0)
            {
                cityService.CreateCity(cityName, countryName);
            }

            var country = dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).First();
            var city = dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).First();
            var bar = new Bar()
            {
                Name = name,
                Address = address,
                Description = description,
                Country = country,
                City = city,
                AverageRating = 0
            };
            dbContext.Bars.Add(bar);
            var barPhoto = new BarPhoto()
            {
                BarCover = barCover,
                Bar = bar
            };
            dbContext.BarPhotos.Add(barPhoto);
            dbContext.SaveChanges();
        }

        // End of Pre-Load

        public async Task AddBarAsync(string name, string address, string description, string countryName, string cityName, byte[] barCover)
        {
            if (!await countryService.CheckIfCountryExists(countryName))
                await countryService.CreateCountryAsync(countryName);

            if (!await cityService.CheckIfCityExistsAsync(cityName))
                await cityService.CreateCityAsync(cityName, countryName);

            var country = await countryService.GetCountryByName(countryName);
            var city = await cityService.GetCityByNameAsync(cityName);
            var bar = new Bar()
            {
                Name = name,
                Address = address,
                Description = description,
                Country = country,
                City = city,
                AverageRating = 0
            };
            await dbContext.Bars.AddAsync(bar);
            var barPhoto = new BarPhoto()
            {
                BarCover = barCover,
                Bar = bar
            };
            await dbContext.BarPhotos.AddAsync(barPhoto);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> BarsCountAsync() =>
            await dbContext.Bars.CountAsync();

        public async Task<IList<string>> GetAllBarNamesAsync() =>
            await dbContext.Bars.Select(p => p.Name).ToListAsync();

        public async Task<IList<string>> GetBarsFromCityAsync(string cityName) =>
            await dbContext.Bars.Where(p => p.City.Name == cityName).Select(p => p.Name).ToListAsync();


        public async Task<byte[]> FindBarPhotoAsync(int id) =>
            (await dbContext.BarPhotos.FirstAsync(p => p.BarId == id)).BarCover;

        public async Task<Bar> FindBarByIdAsync(int id) =>
            await dbContext.Bars.Where(p => p.Id == id).Include(p => p.City).Include(p => p.Country).Include(p => p.Ratings)
                .Include(p => p.Comments).Include(p => p.Cocktails).Include(p => p.FavoritedBy).FirstAsync();


        public async Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder)
        {
            const int pageSize = 10;
            bool lastPage = true;
            var ratings = rating.Split(';');
            var minRating = int.Parse(ratings[0]);
            var maxRating = int.Parse(ratings[1]);
            var bars = dbContext.Bars
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Cocktails)
                .Include(p => p.FavoritedBy)
                .AsQueryable();

            switch (keywordCriteria)
            {
                case "Name":
                    bars = bars.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));
                    break;
                case "Address":
                    bars = bars.Where(p => p.Address.ToLower().Contains(keyword.ToLower()));
                    break;
                case "City":
                    bars = bars.Where(p => p.City.Name.ToLower().Contains(keyword.ToLower()));
                    break;
            }

            bars = bars
                .Where(p => p.AverageRating >= minRating)
                .Where(p => p.AverageRating <= maxRating);

            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.Name);
                    else
                        bars = bars.OrderByDescending(p => p.Name);
                    break;
                case "City":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.City.Name);
                    else
                        bars = bars.OrderByDescending(p => p.City.Name);
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.AverageRating);
                    else
                        bars = bars.OrderByDescending(p => p.AverageRating);
                    break;
                default:
                    break;
            }

            bars = bars.Skip((page - 1) * pageSize);
            var foundBars = await bars.ToListAsync();
            if (foundBars.Count > pageSize)
            {
                lastPage = false;
            }
            foundBars = foundBars.Take(pageSize).ToList();
            return new Tuple<IList<Bar>, bool>(foundBars, lastPage);
        }

        public async Task<IList<Bar>> GetNewestBarsAsync()
        {

            var bars = await dbContext.Bars.Include(p => p.City).Include(p => p.Country).Include(p => p.Ratings)
                 .Include(p => p.Comments).Include(p => p.Cocktails).Include(p => p.FavoritedBy).ToListAsync();
             bars = bars.TakeLast(6).ToList();
            return bars;
        }

    }
}
