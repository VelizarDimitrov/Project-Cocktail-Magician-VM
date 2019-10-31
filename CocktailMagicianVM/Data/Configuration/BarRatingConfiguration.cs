using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
   public class BarRatingConfiguration : IEntityTypeConfiguration<BarRating>
    {
        public void Configure(EntityTypeBuilder<BarRating> builder)
        {
            builder
                .HasOne(p => p.User)
                .WithMany(p => p.BarRatings)
                .HasForeignKey(p => p.UserId);

            builder
                .HasOne(p => p.Bar)
                .WithMany(p => p.Ratings)
                .HasForeignKey(p => p.BarId);

            builder
                .HasKey(p => new { p.BarId, p.UserId });
        }
    }
}
