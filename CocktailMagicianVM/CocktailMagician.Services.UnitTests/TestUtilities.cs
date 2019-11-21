using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CocktailMagician.Services.UnitTests
{
    class TestUtilities
    {
        public static DbContextOptions<CocktailDatabaseContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<CocktailDatabaseContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }
    }
}
