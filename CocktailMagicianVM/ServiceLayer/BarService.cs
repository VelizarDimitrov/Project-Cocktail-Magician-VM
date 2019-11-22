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
        private readonly ICocktailService cocktailService;
        private readonly INotificationService nService;

        public BarService(CocktailDatabaseContext dbContext, ICountryService countryService, ICityService cityService, ICocktailService cocktailService, INotificationService nService)
        {
            this.dbContext = dbContext;
            this.countryService = countryService;
            this.cityService = cityService;
            this.cocktailService = cocktailService;
            this.nService = nService;
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
                AverageRating = 0,
                Hidden = 0
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
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Bar name cannot be null or empty.");
            if (String.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address cannot be null or empty.");
            if (!await countryService.CheckIfCountryExistsAsync(countryName))
                await countryService.CreateCountryAsync(countryName);

            if (!await cityService.CheckIfCityExistsAsync(cityName))
                await cityService.CreateCityAsync(cityName, countryName);

            var country = await countryService.GetCountryByNameAsync(countryName);
            var city = await cityService.GetCityByNameAsync(cityName);
            var bar = new Bar()
            {
                Name = name,
                Address = address,
                Description = description,
                Country = country,
                City = city,
                AverageRating = 0,
                Hidden = 0
            };
            await dbContext.Bars.AddAsync(bar);
            var barPhoto = new BarPhoto()
            {
                BarCover = barCover,
                Bar = bar
            };
            await dbContext.BarPhotos.AddAsync(barPhoto);
            await dbContext.SaveChangesAsync();

            await nService.CityNotification(name, cityName);
        }

        public async Task<int> BarsCountAsync() =>
            await dbContext.Bars.CountAsync();

        public async Task<IList<string>> GetAllBarNamesAsync() =>
            await dbContext.Bars.Select(p => p.Name).ToListAsync();

        public async Task<IList<string>> GetBarsFromCityAsync(string cityName) =>
            await dbContext.Bars.Where(p => p.City.Name == cityName).Select(p => p.Name).ToListAsync();


        public async Task<BarPhoto> FindBarPhotoAsync(int id) =>
            await dbContext.BarPhotos.FirstOrDefaultAsync(p => p.BarId == id);

        public async Task<Bar> FindBarByIdAsync(int id) =>
            await dbContext.Bars
                        .Where(p => p.Id == id)
                        .Include(p => p.City)
                        .Include(p => p.Country)
                        .Include(p => p.Ratings)
                        .Include(p => p.Comments)
                        .Include(p => p.Cocktails)
                        .Include(p => p.FavoritedBy)
                        .FirstOrDefaultAsync();

        public async Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, string keywordCriteria, int page, string selectedOrderBy, string rating, string sortOrder, int pageSize)
        {
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
                .Where(p => p.Hidden == 0)
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
                case "Cocktail":
                    bars = bars.Where(p => p.Cocktails.Any(x => x.CocktailName.ToLower() == keyword.ToLower()));
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

        // rename method and use it from manage and favorite pages 
        public async Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, int page, int pageSize, int? userId = null)
        {
            bool lastPage = true;

            var bars = dbContext.Bars
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.FavoritedBy)
                .AsQueryable();

            if (userId != null)
            {
                bars = bars.Where(p => (p.FavoritedBy.Any(x => x.UserId == userId) && p.Hidden == 0));
            }

            bars = bars.Where(p =>
            p.Name.ToLower().Contains(keyword.ToLower())
            || p.Country.Name.ToLower().Contains(keyword.ToLower())
            || p.City.Name.ToLower().Contains(keyword.ToLower()));

            bars = bars.Skip((page - 1) * pageSize);
            var foundBars = await bars.ToListAsync();

            if (foundBars.Count > pageSize)
            {
                lastPage = false;
            }
            foundBars = foundBars.Take(pageSize).ToList();
            return new Tuple<IList<Bar>, bool>(foundBars, lastPage);
        }

        //public async Task<Tuple<IList<Bar>, bool>> FindBarsForCatalogAsync(string keyword, int page, int pageSize)
        //{
        //    bool lastPage = true;

        //    var bars = dbContext.Bars
        //        .Include(p => p.City)
        //        .Include(p => p.Country)
        //        .AsQueryable();

        //    bars = bars.Where(p =>
        //    p.Name.ToLower().Contains(keyword.ToLower())
        //    || p.Country.Name.ToLower().Contains(keyword.ToLower())
        //    || p.City.Name.ToLower().Contains(keyword.ToLower()));

        //    bars = bars.Skip((page - 1) * pageSize);
        //    var foundBars = await bars.ToListAsync();

        //    if (foundBars.Count > pageSize)
        //    {
        //        lastPage = false;
        //    }
        //    foundBars = foundBars.Take(pageSize).ToList();
        //    return new Tuple<IList<Bar>, bool>(foundBars, lastPage);
        //}

        public async Task<IList<Bar>> GetNewestBarsAsync() =>
            await dbContext.Bars
                        .Include(p => p.City)
                        .Include(p => p.Country)
                        .Include(p => p.Ratings)
                        .Include(p => p.Comments)
                        .Include(p => p.Cocktails)
                        .Include(p => p.FavoritedBy)
                        .Take(6)
                        .ToListAsync();

        public async Task UpdateAverageRatingAsync(int barId)
        {
            var bar = await FindBarByIdAsync(barId);
            bar.AverageRating = Math.Round(bar.Ratings.Average(p => p.Rating), 1);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IList<BarComment>> GetBarCommentsAsync(int barId, int loadNumber) =>
        await dbContext.BarComment
                    .Where(p => p.BarId == barId)
                    .OrderByDescending(p => p.CreatedOn)
                    .Take(6)
                    .ToListAsync();


        public async Task<Bar> FindBarByNameAsync(string barName) =>
            await dbContext.Bars
                            .Where(p => p.Name.ToLower() == barName.ToLower())
                            .Include(p => p.City)
                            .Include(p => p.Country)
                            .Include(p => p.Ratings)
                            .Include(p => p.Comments)
                            .Include(p => p.Cocktails)
                            .Include(p => p.FavoritedBy)
                            .FirstOrDefaultAsync();

        public async Task HideBarAsync(int id)
        {
            var bar = await FindBarByIdAsync(id);

            if (bar != null)
            {
                bar.Hidden = 1;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UnhideBarAsync(int id)
        {
            var bar = await FindBarByIdAsync(id);

            if (bar != null)
            {
                bar.Hidden = 0;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddCocktailBarAsync(int barId, int cocktailId)
        {
            var bar = await FindBarByIdAsync(barId);
            var cocktail = await cocktailService.FindCocktailByIdAsync(cocktailId);

            if (bar != null && cocktail != null)
            {
                var barCocktail = new BarCocktail()
                {
                    Bar = bar,
                    Cocktail = cocktail,
                    CocktailName = cocktail.Name,
                    BarName = bar.Name
                };
                await dbContext.BarCocktail.AddAsync(barCocktail);
                await dbContext.SaveChangesAsync();
            }
            await nService.FavBarNotification(bar.Name, cocktail.Name);
            await nService.FavCocktailNotification(bar.Name,cocktail.Name,bar.City.Name);
        }

        public async Task RemoveCoctailBarAsync(int barId, int cocktailId)
        {
            var barCocktail = await dbContext.BarCocktail.FirstOrDefaultAsync(p => p.BarId == barId && p.CocktailId == cocktailId);

            if (barCocktail != null)
            {
                dbContext.BarCocktail.Remove(barCocktail);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateBarAsync(int id, string name, string address, string description, string countryName, string cityName, byte[] barPhoto)
        {
            var bar = await FindBarByIdAsync(id);
            if (barPhoto!=null)
            {
                var photo = await FindBarPhotoAsync(id);
                photo.BarCover = barPhoto;
            }

            bar.Name = name;
            bar.Address = address;
            bar.Description = description;

            if (!await countryService.CheckIfCountryExistsAsync(countryName))
                await countryService.CreateCountryAsync(countryName);

            if (!await cityService.CheckIfCityExistsAsync(cityName))
                await cityService.CreateCityAsync(cityName, countryName);

            var country = await countryService.GetCountryByNameAsync(countryName);
            var city = await cityService.GetCityByNameAsync(cityName);

            bar.Country = country;
            bar.City = city;
            await dbContext.SaveChangesAsync();

        }
    }
}
