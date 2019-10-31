using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
   public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
       
            builder
                .HasOne(p => p.Country)
                .WithMany(p => p.Cities)
                .HasForeignKey(p => p.CountryId);
        }
    }
}
