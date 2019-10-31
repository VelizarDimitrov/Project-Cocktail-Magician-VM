using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    class UserCocktailConfiguration : IEntityTypeConfiguration<UserCocktail>
    {
        public void Configure(EntityTypeBuilder<UserCocktail> builder)
        {
            builder
                .HasOne(p => p.Cocktail)
                .WithMany(p => p.FavoritedBy)
                .HasForeignKey(p => p.CocktailId);

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.FavoriteCocktails)
                .HasForeignKey(p => p.UserId);

            builder
                .HasKey(p => new { p.UserId, p.CocktailId });
        }
    }
}
