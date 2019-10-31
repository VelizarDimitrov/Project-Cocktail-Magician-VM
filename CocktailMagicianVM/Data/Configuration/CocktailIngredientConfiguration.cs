using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    class CocktailIngredientConfiguration : IEntityTypeConfiguration<CocktailIngredient>
    {
        public void Configure(EntityTypeBuilder<CocktailIngredient> builder)
        {
            builder
                .HasOne(p => p.Cocktail)
                .WithMany(p => p.Ingredients)
                .HasForeignKey(p => p.CocktailId);

            builder
                .HasOne(p => p.Ingredient)
                .WithMany(p => p.Cocktails)
                .HasForeignKey(p => p.IngredientId);

            builder
                .HasKey(p => new { p.IngredientId, p.CocktailId });
        }
    }
}
