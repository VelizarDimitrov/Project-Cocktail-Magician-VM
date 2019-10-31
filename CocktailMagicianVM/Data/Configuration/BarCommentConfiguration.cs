using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    public class BarCommentConfiguration : IEntityTypeConfiguration<BarComment>
    {
        public void Configure(EntityTypeBuilder<BarComment> builder)
        {
            builder
                .HasOne(p => p.User)
                .WithMany(p => p.BarComments)
                .HasForeignKey(p => p.UserId);

            builder
                .HasOne(p => p.Bar)
                .WithMany(p => p.Comments)
                .HasForeignKey(p => p.BarId);

            builder
                .HasKey(p => new { p.BarId, p.UserId });
        }
    }
}
