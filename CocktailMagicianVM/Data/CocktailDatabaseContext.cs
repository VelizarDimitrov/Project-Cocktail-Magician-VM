using Data.Configuration;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
   public class CocktailDatabaseContext : DbContext
    {
        public CocktailDatabaseContext()
        {

        }

        public CocktailDatabaseContext(DbContextOptions<CocktailDatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<Bar> Bars { get; set; }

        public DbSet<BarCocktail> BarCocktail { get; set; }
        public DbSet<BarComment> BarComment { get; set; }

        public DbSet<BarRating> BarRating { get; set; }
        public DbSet<BarPhoto> BarPhotos { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Cocktail> Cocktails { get; set; }

        public DbSet<CocktailComment> CocktailComment { get; set; }

        public DbSet<CocktailIngredient> CocktailIngredient { get; set; }

        public DbSet<CocktailRating> CocktailRating { get; set; }
        public DbSet<CocktailPhoto> CocktailPhotos { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<UserBar> UserBar { get; set; }
        public DbSet<UserCocktail> UserCocktail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=tcp:cocktailmagician.database.windows.net,1433;Initial Catalog=cocktailMagician;User Id=MagicianVM@cocktailmagician.database.windows.net;Password=123abcVM;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BarCocktailConfiguration());
            modelBuilder.ApplyConfiguration(new BarCommentConfiguration());
            modelBuilder.ApplyConfiguration(new BarConfiguration());
            modelBuilder.ApplyConfiguration(new BarRatingConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new CocktailCommentConfiguration());
            modelBuilder.ApplyConfiguration(new CocktailIngredientConfiguration());
            modelBuilder.ApplyConfiguration(new CocktailRatingConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new UserBarConfiguration());
            modelBuilder.ApplyConfiguration(new UserCocktailConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
