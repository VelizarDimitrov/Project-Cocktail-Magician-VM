using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    class BarConfiguration : IEntityTypeConfiguration<Bar>
    {
        public void Configure(EntityTypeBuilder<Bar> builder)
        {
            builder
                .HasOne(p => p.City)
                .WithMany(p => p.Bars)
                .HasForeignKey(p => p.CityId);

            builder
                .HasOne(p => p.Country)
                .WithMany(p => p.Bars)
                .HasForeignKey(p => p.CountryId);
        }
    }
}
