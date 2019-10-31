using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    class CocktailRatingConfiguration : IEntityTypeConfiguration<CocktailRating>
    {
        public void Configure(EntityTypeBuilder<CocktailRating> builder)
        {
            builder
                .HasOne(p => p.Cocktail)
                .WithMany(p => p.Ratings)
                .HasForeignKey(p => p.CocktailId);

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.CocktailRatings)
                .HasForeignKey(p => p.UserId);

            builder
                .HasKey(p => new { p.UserId, p.CocktailId });
        }
    }
}
