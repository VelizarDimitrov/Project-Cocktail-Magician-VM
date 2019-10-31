using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    class UserBarConfiguration : IEntityTypeConfiguration<UserBar>
    {
        public void Configure(EntityTypeBuilder<UserBar> builder)
        {
            builder
                .HasOne(p => p.Bar)
                .WithMany(p => p.FavoritedBy)
                .HasForeignKey(p => p.BarId);

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.FavoriteBars)
                .HasForeignKey(p => p.UserId);

            builder
                .HasKey(p => new { p.UserId, p.BarId });
        }
    }
}
