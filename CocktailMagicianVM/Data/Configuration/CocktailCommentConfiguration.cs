using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    class CocktailCommentConfiguration : IEntityTypeConfiguration<CocktailComment>
    {
        public void Configure(EntityTypeBuilder<CocktailComment> builder)
        {
            builder
                .HasOne(p => p.Cocktail)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.CocktailId);

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.CocktailComments)
                .HasForeignKey(p => p.UserId);

            builder
                .HasKey(p => new { p.UserId, p.CocktailId });
        }
    }
}
