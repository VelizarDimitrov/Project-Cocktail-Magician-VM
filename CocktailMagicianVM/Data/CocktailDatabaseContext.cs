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

        public DbSet<Bar> Bar { get; set; }

        public DbSet<BarCocktail> BarCocktail { get; set; }
        public DbSet<BarComment> BarComment { get; set; }

        public DbSet<BarRating> BarRating { get; set; }

        public DbSet<City> City { get; set; }

        public DbSet<Cocktail> Cocktail { get; set; }

        public DbSet<CocktailComment> CocktailComment { get; set; }

        public DbSet<CocktailIngredient> CocktailIngredient { get; set; }

        public DbSet<CocktailRating> CocktailRating { get; set; }

        public DbSet<Country> Country { get; set; }

        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserBar> UserBar { get; set; }
        public DbSet<UserCocktail> UserCocktail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=CocktailMagician;Trusted_Connection=True;");
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
