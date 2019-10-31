﻿using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasOne(p => p.City)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.CityId);

            builder
                .HasOne(p => p.Country)
                .WithMany(p => p.Users)
                .HasForeignKey(p => p.CountryId);
          
        }
    }
}
