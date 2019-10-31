using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
   public class BarCocktailConfiguration : IEntityTypeConfiguration<BarCocktail>
    {
        public void Configure(EntityTypeBuilder<BarCocktail> builder)
        {
            builder
                .HasOne(p => p.Bar)
                .WithMany(p => p.Cocktails)
                .HasForeignKey(p => p.BarId);

            builder
                .HasOne(p => p.Cocktail)
                .WithMany(p => p.Bars)
                .HasForeignKey(p => p.CocktailId);

            builder
                .HasKey(p => new { p.BarId, p.CocktailId });
        }
    }
}
