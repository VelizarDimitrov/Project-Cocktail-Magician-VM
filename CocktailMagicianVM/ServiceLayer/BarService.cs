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
            if (await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).CountAsync() == 0)
            {
                await countryService.CreateCountryAsync(countryName);
            }

            if (await dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).CountAsync() == 0)
            {
                await cityService.CreateCityAsync(cityName, countryName);
            }

            var country = await dbContext.Countries.Where(p => p.Name.ToLower() == countryName.ToLower()).FirstAsync();
            var city = await dbContext.Cities.Where(p => p.Name.ToLower() == cityName.ToLower()).FirstAsync();
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

        public async Task<int> BarsCountAsync()
        {
            var bars = await dbContext.Bars.CountAsync();
            return bars;
        }

        
        public async Task<IList<string>> GetAllBarNamesAsync()
        {
            var bars = await dbContext.Bars.Select(p => p.Name).ToListAsync();
            return bars;
        }
        public async Task<IList<string>> GetBarsFromCityAsync(string cityName)
        {
            var bars = await dbContext.Bars.Where(p => p.City.Name == cityName).Select(p => p.Name).ToListAsync();
            return bars;
        }
        public async Task<byte[]> FindBarPhotoAsync(int id)
        {
            var photo = await dbContext.BarPhotos.FirstAsync(p => p.BarId == id);
            return photo.BarCover;
        }

        public async Task<Bar> FindBarByIdAsync(int id)
        {
            var bar = await dbContext.Bars.Where(p => p.Id == id)
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Cocktails)
                .Include(p => p.FavoritedBy)
                .FirstAsync();
            return bar;



        }

        public async Task<Tuple<IList<Bar>, bool>> FindBarByNameAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder)
        {
            bool lastPage = true;
            List<Bar> bars;
            var ratings = rating.Split(';');
            var a = int.Parse(ratings[0]);
            var b = int.Parse(ratings[1]);
            bars = await dbContext.Bars
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Cocktails)
                .Include(p => p.FavoritedBy)
               .Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
               .Where(p => p.AverageRating >= a)
               .Where(p => p.AverageRating <= b)
               .ToListAsync();
            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.Name).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.Name).ToList();
                    break;
                case "City":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.City.Name).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.City.Name).ToList();
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.AverageRating).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.AverageRating).ToList();
                    break;
                default:
                    break;
            }
            bars = bars.Skip((page - 1) * 10).ToList();
            if (bars.Count > 10)
            {
                lastPage = false;
            }
            bars = bars.Take(10).ToList();
            return new Tuple<IList<Bar>, bool>(bars, lastPage);
        }

        public async Task<Tuple<IList<Bar>, bool>> FindBarByAddressAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder)
        {
            bool lastPage = true;
            List<Bar> bars;
            var ratings = rating.Split(';');
            var a = int.Parse(ratings[0]);
            var b = int.Parse(ratings[1]);
            bars = await dbContext.Bars
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Cocktails)
                .Include(p => p.FavoritedBy)
               .Where(p => p.Address.ToLower().Contains(keyword.ToLower()))
               .Where(p => p.AverageRating >= a)
               .Where(p => p.AverageRating <= b)
               .ToListAsync();
            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.Name).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.Name).ToList();
                    break;
                case "City":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.City.Name).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.City.Name).ToList();
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.AverageRating).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.AverageRating).ToList();
                    break;
                default:
                    break;
            }
            bars = bars.Skip((page - 1) * 10).ToList();
            if (bars.Count > 10)
            {
                lastPage = false;
            }
            bars = bars.Take(10).ToList();
            return new Tuple<IList<Bar>, bool>(bars, lastPage);
        }

        public async Task<Tuple<IList<Bar>, bool>> FindBarByCityAsync(string keyword, int page, string selectedOrderBy, string rating, string sortOrder)
        {
            bool lastPage = true;
            List<Bar> bars;
            var ratings = rating.Split(';');
            var a = int.Parse(ratings[0]);
            var b = int.Parse(ratings[1]);
            bars = await dbContext.Bars
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Cocktails)
                .Include(p => p.FavoritedBy)
               .Where(p => p.City.Name.ToLower().Contains(keyword.ToLower()))
               .Where(p => p.AverageRating >= a)
               .Where(p => p.AverageRating <= b)
               .ToListAsync();
            switch (selectedOrderBy)
            {
                case "Name":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.Name).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.Name).ToList();
                    break;
                case "City":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.City.Name).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.City.Name).ToList();
                    break;
                case "Rating":
                    if (sortOrder == "Ascending")
                        bars = bars.OrderBy(p => p.AverageRating).ToList();
                    else
                        bars = bars.OrderByDescending(p => p.AverageRating).ToList();
                    break;
                default:
                    break;
            }
            bars = bars.Skip((page - 1) * 10).ToList();
            if (bars.Count > 10)
            {
                lastPage = false;
            }
            bars = bars.Take(10).ToList();
            return new Tuple<IList<Bar>, bool>(bars, lastPage);
        }

        public async Task<IList<Bar>> GetNewestBarsAsync()
        {
            var bars = await dbContext.Bars
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Cocktails)
                .Include(p => p.FavoritedBy).ToListAsync();
            bars = bars.TakeLast(6).ToList();
            return bars;
        }
    }
}
